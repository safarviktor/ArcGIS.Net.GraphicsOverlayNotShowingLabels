using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;

namespace GraphicOverlayLabels
{
    /// <summary>
    /// Provides map data to an application
    /// </summary>
    public class MapViewModel : INotifyPropertyChanged
    {
        private const string FieldName = "Name";

        public MapViewModel()
        {
            _map = new Map(SpatialReferences.WebMercator)
            {
                InitialViewpoint = new Viewpoint(
                    new Envelope(-180, -85, 180, 85, SpatialReferences.Wgs84)
                ),
                Basemap = new Basemap(BasemapStyle.ArcGISStreets)
            };

            string expressionScript = $"$feature." + FieldName;
            var expression = new ArcadeLabelExpression(expressionScript);

            var labelDef = new LabelDefinition(
                expression,
                new TextSymbol()
                {
                    Color = System.Drawing.Color.Black,
                    Size = 10,
                    HaloColor = System.Drawing.Color.White,
                    HaloWidth = 2
                }
            );

            var g = new Graphic() { Geometry = new MapPoint(0, 0, SpatialReferences.Wgs84) };
            g.Attributes.Add(FieldName, "Homer");

            var table = new FeatureCollectionTable([g], GetFields());
            var collectionLayer = new FeatureCollectionLayer(new FeatureCollection([table]))
            {
                Id = "Test collection",
            };
            var layer = collectionLayer.Layers[0];
            layer.Id = "Testlayer";
            layer.Name = "Test layer";
            layer.Renderer = GetRenderer();
            layer.LabelsEnabled = true;
            layer.LabelDefinitions.Add(labelDef);
            Map.OperationalLayers.Add(collectionLayer);

            //var go = new GraphicsOverlay() { LabelsEnabled = true, Id = "test labels" };
            //go.LabelDefinitions.Add(labelDef);
            //go.Renderer = GetRenderer();
            //go.Graphics.Add(g);
            //GraphicsOverlays.Add(go);
        }

        private Renderer GetRenderer()
        {
            return new SimpleRenderer(
                new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Red, 20)
            );
        }

        private List<Field> GetFields()
        {
            return [new Field(FieldType.Text, FieldName, null, 20)];
        }

        private Map _map;

        /// <summary>
        /// Gets or sets the map
        /// </summary>
        public Map Map
        {
            get => _map;
            set
            {
                _map = value;
                OnPropertyChanged();
            }
        }

        private GraphicsOverlayCollection _graphicsOverlays = [];

        public GraphicsOverlayCollection GraphicsOverlays
        {
            get => _graphicsOverlays;
            set
            {
                _graphicsOverlays = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}