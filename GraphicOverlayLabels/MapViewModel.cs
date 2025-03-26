using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public MapViewModel()
        {
            _map = new Map(SpatialReferences.WebMercator)
            {
                InitialViewpoint = new Viewpoint(
                    new Envelope(-180, -85, 180, 85, SpatialReferences.Wgs84)
                ),
                // Basemap = new Basemap(BasemapStyle.ArcGISStreets)
            };

            var labelDef = new LabelDefinition(
                new SimpleLabelExpression("Some text"),
                new TextSymbol()
                {
                    Color = System.Drawing.Color.Red,
                    Size = 40,
                    HaloColor = System.Drawing.Color.White,
                    HaloWidth = 2
                }
            );

            var go = new GraphicsOverlay() { LabelsEnabled = true, Id = "test labels" };
            go.LabelDefinitions.Add(labelDef);
            go.Renderer = new SimpleRenderer(
                new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Red, 20)
            );

            var g = new Graphic() { Geometry = new MapPoint(0, 0) };
            g.Attributes.Add("name", "Homer");
            go.Graphics.Add(g);

            _graphicsOverlays.Add(go);
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
