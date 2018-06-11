// Copyright 2017 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
// language governing permissions and limitations under the License.

using Android.App;
using Android.OS;
using Android.Widget;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Collections.Generic;

namespace ArcGISRuntime.Samples.WMTSLayer
{
    [Activity]
    [ArcGISRuntime.Samples.Shared.Attributes.Sample(
        "WMTS layer",
        "Layers",
        "This sample demonstrates how to display a WMTS layer on a map via a Uri and WmtsLayerInfo.",
        "")]
    public class WMTSLayer : Activity
    {
        // Create and hold reference to the used MapView
        private MapView _myMapView = new MapView();

        private Button _infoButton;
        private Button _uriButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Title = "WMTS layer";

            // Create the UI, setup the control references
            CreateLayout();

            // Load the map using Uri to the WMTS service.
            UriButton_Clicked(null, null);
        }

        private void UriButton_Clicked(object sender, EventArgs e)
        {
            // Create dialog to display alert information
            var alert = new AlertDialog.Builder(this);

            try
            {
                // Define the Uri to the WMTS service
                var myUri = new Uri("http://sampleserver6.arcgisonline.com/arcgis/rest/services/WorldTimeZones/MapServer/WMTS");

                // Create a new instance of a WMTS layer using a Uri and provide an Id value
                WmtsLayer myWmtsLayer = new WmtsLayer(myUri, "WorldTimeZones");

                // Create a new map
                Map myMap = new Map();

                // Get the basemap from the map
                Basemap myBasemap = myMap.Basemap;

                // Get the layer collection for the base layers
                LayerCollection myLayerCollection = myBasemap.BaseLayers;

                // Add the WMTS layer to the layer collection of the map
                myLayerCollection.Add(myWmtsLayer);

                // Assign the map to the MapView
                _myMapView.Map = myMap;

                // Disable and enable the appropriate buttons.
                _uriButton.Enabled = false;
                _infoButton.Enabled = true;
            }
            catch (Exception ex)
            {
                alert.SetTitle("Sample Error");
                alert.SetMessage(ex.Message);
                alert.Show();
            }
        }

        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            // Create dialog to display alert information
            var alert = new AlertDialog.Builder(this);

            try
            {
                // Define the Uri to the WMTS service
                var myUri = new Uri("http://sampleserver6.arcgisonline.com/arcgis/rest/services/WorldTimeZones/MapServer/WMTS");

                // Define a new instance of the WMTS service
                WmtsService myWmtsService = new WmtsService(myUri);

                // Load the WMTS service
                await myWmtsService.LoadAsync();

                // Get the service information (i.e. metadata) about the WMTS service
                WmtsServiceInfo myWMTSServiceInfo = myWmtsService.ServiceInfo;

                // Obtain the read only list of WMTS layer info objects
                IReadOnlyList<WmtsLayerInfo> myWmtsLayerInfos = myWMTSServiceInfo.LayerInfos;

                // Create a new instance of a WMTS layer using the first item in the read only list of WMTS layer info objects
                WmtsLayer myWmtsLayer = new WmtsLayer(myWmtsLayerInfos[0]);

                // Create a new map
                Map myMap = new Map();

                // Get the basemap from the map
                Basemap myBasemap = myMap.Basemap;

                // Get the layer collection for the base layers
                LayerCollection myLayerCollection = myBasemap.BaseLayers;

                // Add the WMTS layer to the layer collection of the map
                myLayerCollection.Add(myWmtsLayer);

                // Assign the map to the MapView
                _myMapView.Map = myMap;

                // Disable and enable the appropriate buttons.
                _uriButton.Enabled = true;
                _infoButton.Enabled = false;
            }
            catch (Exception ex)
            {
                alert.SetTitle("Sample Error");
                alert.SetMessage(ex.Message);
                alert.Show();
            }
        }

        private void CreateLayout()
        {
            // Create a new vertical layout for the app
            var layout = new LinearLayout(this) { Orientation = Orientation.Vertical };

            // Create Button
            _uriButton = new Button(this);
            _uriButton.Text = "WMTSLayer via Uri";
            _uriButton.Click += UriButton_Clicked;

            // Add Button to the layout
            layout.AddView(_uriButton);

            // Create Button
            _infoButton = new Button(this);
            _infoButton.Text = "WMTSLayer via WmtsLayerInfo";
            _infoButton.Click += InfoButton_Clicked;

            // Add Button to the layout
            layout.AddView(_infoButton);

            // Add the map view to the layout
            layout.AddView(_myMapView);

            // Show the layout in the app
            SetContentView(layout);
        }
    }
}