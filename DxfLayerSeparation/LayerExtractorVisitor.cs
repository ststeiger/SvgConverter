using System.Collections.Generic;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;

namespace WW.Cad.Example
{


    /// <summary>
    /// Remove all layers but one from a model.
    /// </summary>
    public class LayerExtractorVisitor : BasicEntityVisitor
    {
        /// <summary>
        /// Info of the insertion.
        /// </summary>
        private class InsertInfo
        {
            /// <summary>
            /// Effective layer from insertion.
            /// </summary>
            public readonly DxfLayer effectiveLayer;
            /// <summary>
            /// Currentky active entity collection.
            /// </summary>
            public readonly DxfEntityCollection currentCollection;
            /// <summary>
            /// Currently active index, used for removal.
            /// </summary>
            public int currentIndex;
            /// <summary>
            /// Did we encounter zero layer entities?
            /// </summary>
            private bool hasZeroLayerEntities;

            /// <summary>
            /// Initializes a new instance of the <see cref="InsertInfo"/> class.
            /// </summary>
            /// <param name="currentCollection">The current collection.</param>
            /// <param name="effectiveLayer">The effective insertion layer.</param>
            public InsertInfo(DxfEntityCollection currentCollection, DxfLayer effectiveLayer)
            {
                this.effectiveLayer = effectiveLayer;
                this.currentCollection = currentCollection;
                currentIndex = currentCollection.Count - 1;
            }

            /// <summary>
            /// Gets the effective layer for an entity.
            /// </summary>
            /// <param name="entity">The entity.</param>
            /// <returns>The effective layer.</returns>
            public DxfLayer GetEffectiveLayer(DxfEntity entity)
            {
                if (entity.Layer.IsZeroLayer)
                {
                    hasZeroLayerEntities = true;
                    return effectiveLayer;
                }
                return entity.Layer;
            }

            /// <summary>
            /// Gets a value indicating whether zero layer entities were encountered.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance has zero layer entities; otherwise, <c>false</c>.
            /// </value>
            public bool HasZeroLayerEntities
            {
                get { return hasZeroLayerEntities; }
            }
        }


        /// <summary>
        /// Helper class: block and the possibly two reduced blocks.
        /// </summary>
        private class BlockData
        {
            /// <summary>
            /// Back ref to outer class.
            /// </summary>
            private readonly LayerExtractorVisitor visitor;
            /// <summary>
            /// The original block.
            /// </summary>
            private readonly DxfBlock original;
            /// <summary>
            /// The block inserted with the keep layer.
            /// </summary>
            private DxfBlock blockWithKeepLayerInsert;
            /// <summary>
            /// The block inserted with a layer different from the keep layer.
            /// </summary>
            private DxfBlock blockWithDifferentLayerInsert;

            /// <summary>
            /// Initializes a new instance of the <see cref="BlockData"/> class.
            /// </summary>
            /// <param name="visitor">Back reference to outer class.</param>
            /// <param name="block">The original block.</param>
            public BlockData(LayerExtractorVisitor visitor, DxfBlock block)
            {
                this.visitor = visitor;
                original = block;
            }

            /// <summary>
            /// Gets the block depending on the effective layer of the insert.
            /// </summary>
            /// <param name="insertLayer">The insert layer.</param>
            /// <returns>The block.</returns>
            public DxfBlock GetBlock(DxfLayer insertLayer)
            {
                return (ReferenceEquals(insertLayer, visitor.keepLayer))
                           ? WithKeepLayer
                           : WithDifferentLayer(insertLayer);
            }

            /// <summary>
            /// Get the block for an insertion with the keep layer.
            /// </summary>
            private DxfBlock WithKeepLayer
            {
                get
                {
                    if (blockWithKeepLayerInsert == null)
                    {
                        // CloneContext cc = new CloneContext(visitor.model, ReferenceResolutionType.CloneMissing);
                        CloneContext cc = new CloneContext(visitor.model, visitor.model, ReferenceResolutionType.CloneMissing);

                        blockWithKeepLayerInsert = (DxfBlock)original.Clone(cc);
                        cc.ResolveReferences();
                        if (blockWithDifferentLayerInsert != null)
                        {
                            // need different name for block
                            blockWithKeepLayerInsert.Name += "-KEEP";
                        }
                        if (!visitor.HandleEntityCollection(blockWithKeepLayerInsert.Entities,
                                                            visitor.insertStack.Peek().effectiveLayer))
                        {
                            // no layer 0 entities, so no need to have diffent blocks
                            blockWithDifferentLayerInsert = blockWithKeepLayerInsert;
                        }
                    }
                    return blockWithKeepLayerInsert;
                }
            }

            /// <summary>
            /// Get the block with an insertion with a layer different from the keep layer.
            /// </summary>
            /// <param name="insertLayer">The insert layer.</param>
            /// <returns>The block.</returns>
            private DxfBlock WithDifferentLayer(DxfLayer insertLayer)
            {
                if (blockWithDifferentLayerInsert == null)
                {
                    // CloneContext cc = new CloneContext(visitor.model, ReferenceResolutionType.CloneMissing);
                    CloneContext cc = new CloneContext(visitor.model, visitor.model, ReferenceResolutionType.CloneMissing);

                    blockWithDifferentLayerInsert = (DxfBlock)original.Clone(cc);
                    cc.ResolveReferences();
                    if (blockWithKeepLayerInsert != null)
                    {
                        // need different name for block
                        blockWithDifferentLayerInsert.Name += "-DIFF";
                    }
                    if (!visitor.HandleEntityCollection(blockWithDifferentLayerInsert.Entities,
                                                        insertLayer))
                    {
                        // no layer 0 entitites, so no need to have diffent blocks
                        blockWithKeepLayerInsert = blockWithDifferentLayerInsert;
                    }
                }
                return blockWithDifferentLayerInsert;
            }

            /// <summary>
            /// Get all blocks created from 
            /// </summary>
            public List<DxfBlock> Blocks
            {
                get
                {
                    List<DxfBlock> result = new List<DxfBlock>();
                    if (blockWithKeepLayerInsert != null)
                    {
                        result.Add(blockWithKeepLayerInsert);
                    }
                    if (blockWithDifferentLayerInsert != null && !ReferenceEquals(blockWithKeepLayerInsert, blockWithDifferentLayerInsert))
                    {
                        result.Add(blockWithDifferentLayerInsert);
                    }
                    return result;
                }
            }
        }
        /// <summary>
        /// Current insertion stack, the top has the active information.
        /// </summary>
        private readonly Stack<InsertInfo> insertStack = new Stack<InsertInfo>();
        /// <summary>
        /// The model.
        /// </summary>
        private DxfModel model;
        /// <summary>
        /// The layer to keep.
        /// </summary>
        private DxfLayer keepLayer;
        /// <summary>
        /// Mapping of block names to block data.
        /// </summary>
        private readonly IDictionary<string, BlockData> blockDataByName = new Dictionary<string, BlockData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerExtractorVisitor"/> class.
        /// </summary>
        /// <remarks>
        /// This will iterate over the model and remove all entities not on the keep layer.
        /// </remarks>
        /// <param name="model">The model.</param>
        /// <param name="keepLayer">The keep layer.</param>
        public void Run(DxfModel model, DxfLayer keepLayer)
        {
            insertStack.Clear();
            blockDataByName.Clear();
            this.model = model;
            this.keepLayer = keepLayer;
            foreach (DxfBlock block in model.Blocks)
            {
                blockDataByName.Add(block.Name, new BlockData(this, block));
            }

            // run removal
            HandleEntityCollection(model.Entities, model.ZeroLayer);

            // take care of blocks
            foreach (BlockData blockData in blockDataByName.Values)
            {
                foreach (DxfBlock block in blockData.Blocks)
                {
                    DxfBlock oldBlock;
                    if (model.Blocks.TryGetValue(block.Name, out oldBlock))
                    {
                        model.Blocks.Remove(oldBlock);
                    }
                    model.Blocks.Add(block);
                }
            }

            // remove all unused layers
            for (int l = model.Layers.Count - 1; l >= 0; --l)
            {
                DxfLayer layer = model.Layers[l];
                if (!layer.IsZeroLayer && IsRemovedLayer(layer))
                {
                    model.Layers.RemoveAt(l);
                }
            }

            // just to be sure
            // model.Repair();
            System.Collections.Generic.List<WW.Cad.Base.DxfMessage> ls = new System.Collections.Generic.List<WW.Cad.Base.DxfMessage>();
            model.Repair(ls);
        }

        /// <summary>
        /// Is a layer removed?
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>The answer.</returns>
        private bool IsRemovedLayer(DxfLayer layer)
        {
            // remove ! in order to remove the keep layer
            return !ReferenceEquals(layer, keepLayer);
        }

        /// <summary>
        /// Run over a collection of entities and remove all with different layers.
        /// </summary>
        /// <param name="entities">Entity collection.</param>
        /// <param name="effectiveLayer">The effective layer.</param>
        /// <returns><c>true</c> if zero layer entities were included in this collection.</returns>
        private bool HandleEntityCollection(DxfEntityCollection entities, DxfLayer effectiveLayer)
        {
            InsertInfo currentInfo = new InsertInfo(entities, effectiveLayer);
            try
            {
                insertStack.Push(currentInfo);
                while (currentInfo.currentIndex >= 0)
                {
                    DxfEntity entity = currentInfo.currentCollection[currentInfo.currentIndex];
                    entity.Accept(this);
                    --currentInfo.currentIndex;
                }
            }
            finally
            {
                insertStack.Pop();
            }
            return currentInfo.HasZeroLayerEntities;
        }

        /// <summary>
        /// Handles a simple entity.
        /// </summary>
        /// <remarks>
        /// This just evaluates the effective layer and removes the entity if the layer is not the keep layer.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the entity was removed.</returns>
        private bool HandleEntity(DxfEntity entity)
        {
            InsertInfo info = insertStack.Peek();
            DxfLayer effectiveLayer = info.GetEffectiveLayer(entity);
            if (IsRemovedLayer(effectiveLayer))
            {
                // remove entity on incorrect index
                info.currentCollection.RemoveAt(info.currentIndex);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handle a dimension entity.
        /// </summary>
        /// <remarks>
        /// Same as <see cref="HandleEntity"/>, but removes the associated block, too, if the dimension is removed.
        /// </remarks>
        /// <param name="dim">Dimension entity.</param>
        private void HandleDimension(DxfDimension dim)
        {
            if (HandleEntity(dim))
            {
                // can remove block, too
                DxfBlock block = dim.Block;
                if (block != null)
                {
                    model.Blocks.Remove(block);
                }
            }
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(Dxf3DFace face)
        {
            HandleEntity(face);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(Dxf3DSolid solid)
        {
            HandleEntity(solid);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfArc arc)
        {
            HandleEntity(arc);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfAttribute attribute)
        {
            HandleEntity(attribute);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfAttributeDefinition attributeDefinition)
        {
            HandleEntity(attributeDefinition);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfCircle circle)
        {
            HandleEntity(circle);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Aligned dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Angular3Point dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Angular4Point dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Diametric dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Linear dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Ordinate dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfDimension.Radial dimension)
        {
            HandleDimension(dimension);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfEllipse ellipse)
        {
            HandleEntity(ellipse);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfHatch hatch)
        {
            HandleEntity(hatch);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfImage image)
        {
            HandleEntity(image);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfInsert insert)
        {
            InsertInfo info = insertStack.Peek();
            BlockData blockData = blockDataByName[insert.Block.Name];
            DxfLayer effectiveLayer = info.GetEffectiveLayer(insert);
            DxfBlock block = blockData.GetBlock(effectiveLayer);
            if (block.Entities.Count == 0)
            {
                // no need to keep insert if block is empty
                info.currentCollection.RemoveAt(info.currentIndex);
            }
            else
            {
                // overwrite block with reduced one
                insert.Block = block;
                // take care of attributes
                for (int a = insert.Attributes.Count - 1; a >= 0; --a)
                {
                    DxfAttribute attrib = insert.Attributes[a];
                    DxfLayer attribLayer = attrib.Layer.IsZeroLayer
                                               ? effectiveLayer
                                               : attrib.Layer;
                    if (IsRemovedLayer(attribLayer))
                    {
                        insert.Attributes.RemoveAt(a);
                    }
                }
            }
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfLeader leader)
        {
            HandleEntity(leader);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfLine line)
        {
            HandleEntity(line);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfLwPolyline polyline)
        {
            HandleEntity(polyline);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfMLine mline)
        {
            HandleEntity(mline);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfMText mtext)
        {
            HandleEntity(mtext);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPoint point)
        {
            HandleEntity(point);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolyfaceMesh mesh)
        {
            HandleEntity(mesh);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolygonMesh mesh)
        {
            HandleEntity(mesh);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolygonSplineMesh mesh)
        {
            HandleEntity(mesh);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolyline2D polyline)
        {
            HandleEntity(polyline);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolyline2DSpline polyline)
        {
            HandleEntity(polyline);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolyline3D polyline)
        {
            HandleEntity(polyline);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfPolyline3DSpline polyline)
        {
            HandleEntity(polyline);  // don't care for vertices, as they ought to have the same layer 
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfRay ray)
        {
            HandleEntity(ray);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfRegion region)
        {
            HandleEntity(region);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfShape shape)
        {
            HandleEntity(shape);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfSolid solid)
        {
            HandleEntity(solid);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfSpline spline)
        {
            HandleEntity(spline);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfTable table)
        {
            if (HandleEntity(table))
            {
                // can remove associated block, too
                DxfBlock block = table.Block;
                if (block != null)
                {
                    model.Blocks.Remove(block);
                }
            }
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfText text)
        {
            HandleEntity(text);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfTolerance tolerance)
        {
            HandleEntity(tolerance);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfViewport viewport)
        {
            HandleEntity(viewport);
        }

        /// <summary>
        /// Visits the specified entity.
        /// See the <see cref="IEntityVisitor"/> for more details.
        /// </summary>
        public override void Visit(DxfXLine xline)
        {
            HandleEntity(xline);
        }
    }


}
