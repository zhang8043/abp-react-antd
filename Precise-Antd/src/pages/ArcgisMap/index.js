import React, { Component } from 'react';
import * as ReactDOM from 'react-dom';
import { Card, Button, Icon } from 'antd';
import styles from './index.less';
import esriLoader from 'esri-loader'

class ArcgisMap extends Component {

    constructor(props) {
        super(props)
        this.tileMapUrl = "https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer"
        this.state = {
            width: props.width || -1,
            height: props.height || -1,
        }
    }

    componentDidMount() {
        this.updateSize();
        window.addEventListener('resize', () => this.updateSize());
        this.initMap();
    }

    componentWillUnmount() {
        window.removeEventListener('resize', () => this.updateSize());
    }

    updateSize() {
        try {
            const parentDom = ReactDOM.findDOMNode(this).parentNode;
            let { width, height } = this.props;
            //如果props没有指定height和width就自适应
            width = "100%";
            if (!height) {
                height = parentDom.offsetHeight - 64;
            }
            this.setState({ width, height });
        } catch (ignore) {
        }
    }

    initMap() {
        const mapURL = {
            url: "http://127.0.0.1:86/arcgis_js_api/library/4.10/dojo/dojo.js"
        }
        esriLoader.loadModules([
            "esri/Map",
            "esri/Basemap",
            "esri/layers/TileLayer",
            "esri/views/MapView",
            "esri/views/SceneView",
            "esri/widgets/BasemapToggle",
            "esri/layers/MapImageLayer",
            "dojo/domReady!"
        ], mapURL).then(([
            Map,
            Basemap,
            TileLayer,
            MapView,
            SceneView,
            BasemapToggle,
            MapImageLayer]) => {

            let layer = new TileLayer({
                url: this.tileMapUrl
            })

            let baseMap = new Basemap({
                baseLayers: [layer],
                title: "自定义底图",
                id: "myBasemap"
            });

            // 创建一个Map实例
            let map = new Map({
                basemap: baseMap,
            });

            // 创建SceneView实例并引用地图实例
            let view = new MapView({
                center: [-117.18, 34.06],
                map: map,
                container: "mapDiv",
                zoom: 5
            });

            var toggle = new BasemapToggle({
                //设置属性
                view: view, // 提供对地图'topo'底图的访问的视图
                nextBasemap: "hybrid" // 允许切换到“混合”底图
            });

            view.ui.add(toggle, "bottom-right");
        })
    }
    render() {
        return (
            <div>
                <div id="mapDiv" style={this.state}></div>
            </div>
        )
    }
}

export default ArcgisMap;
