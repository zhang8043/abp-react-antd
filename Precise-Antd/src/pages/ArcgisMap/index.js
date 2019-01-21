import React, { Component } from 'react';
import * as ReactDOM from 'react-dom';
import { Card, Drawer, Form, Button, Col, Row, Input, Select, DatePicker, Icon, } from 'antd';
import styles from './index.less';
import esriLoader from 'esri-loader'

import dxsj from '@/assets/map/dxsj.png';
import qxsj from '@/assets/map/qxsj.png';
import slgc from '@/assets/map/slgc.png';
import swsj from '@/assets/map/swsj.png';
import tdsj from '@/assets/map/tdsj.png';
import trsj from '@/assets/map/trsj.png';
import zbsj from '@/assets/map/zbsj.png';

@Form.create()
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

    openQueryMenu = (res) => {
        console.log(res);
        this.setState({
            visible: true,
        });
    };

    onClose = () => {
        this.setState({
            visible: false,
        });
    };

    render() {

        const {
            form: { getFieldDecorator },
        } = this.props;


        return (
            <div>
                <div id="mapDiv" style={this.state}></div>
                <div id="container" className={styles.container}>
                    <div id="dock">
                        <ul>
                            <li>
                                <span>气象数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('qxsj')}><img src={qxsj}></img></a>
                            </li>
                            <li>
                                <span>水文数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('swsj')}><img src={swsj}></img></a>
                            </li>
                            <li>
                                <span>植被数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('zbsj')}><img src={zbsj}></img></a>
                            </li>
                            <li>
                                <span>土壤数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('trsj')}><img src={trsj}></img></a>
                            </li>
                            <li>
                                <span>地形数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('dxsj')}><img src={dxsj}></img></a>
                            </li>
                            <li>
                                <span>土地利用数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('tdsj')}><img src={tdsj}></img></a>
                            </li>
                            <li>
                                <span>水利工程数据</span>
                                <a href="javascript:void(0);" onClick={() => this.openQueryMenu('slgc')}><img src={slgc}></img></a>
                            </li>
                        </ul>
                    </div>
                </div>
                <Drawer
                    title="数据查询"
                    placement="left"
                    width={350}
                    closable={false}
                    onClose={this.onClose}
                    visible={this.state.visible}
                >
                    <Form layout="vertical" hideRequiredMark>
                        <Row gutter={16}>
                            <Col span={12}>
                                <Form.Item label="Name">
                                    {getFieldDecorator('name', {
                                        rules: [{ required: true, message: 'Please enter user name' }],
                                    })(<Input placeholder="Please enter user name" />)}
                                </Form.Item>
                            </Col>
                            <Col span={12}>
                                <Form.Item label="Url">
                                    {getFieldDecorator('url', {
                                        rules: [{ required: true, message: 'Please enter url' }],
                                    })(
                                        <Input
                                            style={{ width: '100%' }}
                                            addonBefore="http://"
                                            addonAfter=".com"
                                            placeholder="Please enter url"
                                        />
                                    )}
                                </Form.Item>
                            </Col>
                        </Row>
                    </Form>
                </Drawer>
            </div>
        )
    }
}

export default ArcgisMap;
