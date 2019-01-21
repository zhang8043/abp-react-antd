import React, { Component, PureComponent } from 'react';
import { connect } from 'dva';
import { Tree, Icon, notification, Modal, Button, Row, Col, Card, Form, Input, message, Menu, Table, Dropdown } from 'antd';
import { createTree } from '@/utils/utils';
import PageHeaderWrapper from '@/components/PageHeaderWrapper';
import CreateOrUpdateForm from './CreateOrEditUnit';
import AddMember from './AddMember';
import moment from 'moment';

const { TreeNode } = Tree;
const confirm = Modal.confirm;


@connect(({ loading, organization }) => ({
    organizationLoading: loading.effects['organization/getOrganizationUnits'],
    organizationUnitUsersLoading: loading.effects['organization/getOrganizationUnitUsers'],
    findUsersLoading: loading.effects['organization/findUsers'],
    organization,
}))
class OrganizationUnitsList extends Component {

    state = {
        //是否显示对话框
        modalVisible: false,
        adduserModalVisible: false,
        //右键菜单位置
        rightClickNodeTreeItem: null,
        //右键实体
        formVals: {},
        //右键按钮类型
        modalType: null,
        //获取组织用户参数
        tableParameters: {
            id: null,
            MaxResultCount: 6,
            SkipCount: 0,
            filter: ""
        },
        //组织用户标题
        userCardTitle: null,
    };

    componentDidMount() {
        this.getOrganizationUnits();
    }

    getOrganizationUnits() {
        this.props.dispatch({
            type: 'organization/getOrganizationUnits',
        });
    }

    getOrganizationUnitUsers() {
        this.props.dispatch({
            type: 'organization/getOrganizationUnitUsers',
            payload: this.state.tableParameters,
        });
    }

    //拖动组织
    onDrop = (info) => {
        const { organization: { organizationData }, dispatch } = this.props;
        const dropKey = info.node.props.eventKey;
        const dragKey = info.dragNode.props.eventKey;

        const loop = (data, key, callback) => {
            data.forEach((item, index, arr) => {
                if (item.key === key) {
                    return callback(item, index, arr);
                }
                if (item.children) {
                    return loop(item.children, key, callback);
                }
            });
        };
        const data = [...organizationData];

        let dragObj;
        loop(data, dragKey, (item, index, arr) => {
            arr.splice(index, 1);
            dragObj = item;
        });

        loop(data, dropKey, (item) => {
            item.children = item.children || [];
            confirm({
                title: '确认移动?',
                content: '您确认移动 ' + dragObj.displayName + ' 到 ' + item.displayName,
                okText: '确认',
                cancelText: '取消',
                onOk() {
                    dispatch({
                        type: 'organization/moveOrganizationUnit',
                        payload: { Id: dragKey, NewParentId: dropKey }
                    });
                },
                onCancel() {

                },
            });
        });
    }

    //组织右键
    onRightClick = (e) => {
        const positionInfo = e.event.currentTarget.getBoundingClientRect();
        const x = positionInfo.right;
        const y = positionInfo.bottom;
        this.setState({
            rightClickNodeTreeItem: {
                pageX: x,
                pageY: y,
            },
            formVals: {
                id: e.node.props.eventKey,
                parentId: e.node.props.eventKey,
                displayName: e.node.props.displayName
            }
        });
    };

    //右键菜单
    getNodeTreeRightClickMenu() {
        const { pageX, pageY } = { ...this.state.rightClickNodeTreeItem };
        const tmpStyle = {
            position: 'absolute',
            left: `${pageX - 310}px`,
            top: `${pageY - 190}px`,
            border: '1px solid #c0c0c0',
        };
        const menu = (
            <Menu
                onClick={this.handleMenuClick}
                style={tmpStyle}
                subMenuCloseDelay={0.1}
            >
                <Menu.Item key='add'><Icon type='plus-circle-o' />{'添加子组织'}</Menu.Item>
                <Menu.Item key='update'><Icon type='edit' />{'修改'}</Menu.Item>
                <Menu.Item key='delete'><Icon type='minus-circle-o' />{'删除'}</Menu.Item>
            </Menu>
        );
        return (this.state.rightClickNodeTreeItem === null) ? '' : menu;
    }
    //点击菜单
    handleMenuClick = (e) => {
        const { formVals } = this.state;
        if (e.key === "delete") {
            const { dispatch } = this.props;
            dispatch({
                type: 'organization/deleteOrganizationUnit',
                payload: formVals
            });
            this.setState({
                userCardTitle:null
            });
        } else {
            this.handleModalVisible(true, formVals, e.key);
        }
        this.setState({
            rightClickNodeTreeItem: null,
        });
    }
    //点击组织机构
    onSelectClick = (selectedKeys, e) => {
        this.setState({
            userCardTitle: e.node.props.displayName,
            tableParameters: {
                id: e.node.props.eventKey,
                MaxResultCount: 6,
                SkipCount: 0,
            },
        }, function () {
            this.getOrganizationUnitUsers();
        });
    }
    //对话框控制
    handleModalVisible = (flag, record, modalType) => {
        this.setState({
            modalVisible: !!flag,
            formVals: record || {},
            modalType: modalType || null
        });
    };

    addMemberModalVisible = (flag) => {
        this.setState({
            adduserModalVisible: !!flag,
        });
        const { tableParameters } = this.state;
        tableParameters.organizationUnitId = tableParameters.id;
        this.props.dispatch({
            type: 'organization/findUsers',
            payload: tableParameters,
        });
    };

    //删除组织用户
    removeUser = userId => {
        const { dispatch } = this.props;
        const { tableParameters } = this.state;
        dispatch({
            type: 'organization/removeUserFromOrganizationUnit',
            payload: { UserId: userId, OrganizationUnitId: tableParameters.id },
            callback: (res) => {
                if (res) {
                    this.getOrganizationUnits();
                    this.getOrganizationUnitUsers();
                    message.success('操作成功');
                }
            }
        });
    }

    handleTableChange = (pagination) => {
        const { tableParameters } = this.state;
        tableParameters.SkipCount = (pagination.current - 1) * tableParameters.MaxResultCount;
        this.setState({ tableParameters: tableParameters }, this.getAuditLogs());
    };

    render() {
        const loop = data => data.map((item) => {
            if (item.children && item.children.length) {
                return <TreeNode key={item.key} title={item.displayName} displayName={item.data.displayName} parentId={item.data.parentId}>{loop(item.children)}</TreeNode>;
            }
            return <TreeNode key={item.key} title={item.displayName} displayName={item.data.displayName} />;
        });

        const columns = [
            {
                title: '删除', key: 'action',
                render: (value, row, index) => {
                    return (
                        <Button type="danger" shape="circle" onClick={() => this.removeUser(value.id)} icon="close" />
                    );
                },
            },
            { title: '姓名', dataIndex: 'name', key: 'name', },
            { title: '用户名', dataIndex: 'userName', key: 'userName', },
            { title: '邮箱地址', dataIndex: 'emailAddress', key: 'emailAddress', },
            {
                title: '创建时间', dataIndex: 'creationTime', key: 'creationTime',
                render: (value, row, index) => {
                    return moment(value).format('YYYY-MM-D h:mm:ss');
                },
            }
        ];

        const {
            organizationLoading,
            organizationUnitUsersLoading,
            findUsersLoading,
            organization: { organizationData, organizationUnitUsersData },
        } = this.props;

        const parentMethods = {
            handleModalVisible: this.handleModalVisible,
        };

        const {
            modalType,
            formVals,
            modalVisible,
            adduserModalVisible,
            userCardTitle,
            tableParameters
        } = this.state;

        return (
            <PageHeaderWrapper title="组织机构">
                <Row gutter={16}>
                    <Col span={8}>
                        <Card bordered={false} title="组织结构树" extra={<Button type="primary" onClick={() => this.handleModalVisible(true)}>添加根组织</Button>}>
                            <CreateOrUpdateForm
                                {...parentMethods}
                                modalType={modalType}
                                formVals={formVals}
                                modalVisible={modalVisible}
                            />
                            <Tree
                                className="draggable-tree"
                                draggable
                                autoExpandParent
                                defaultExpandAll
                                onRightClick={this.onRightClick}
                                onDrop={this.onDrop}
                                onSelect={this.onSelectClick}
                            >
                                {loop(organizationData)}
                            </Tree>
                            {this.getNodeTreeRightClickMenu()}
                        </Card>
                    </Col>
                    <Col span={16}>
                        <Card bordered={false}
                            title={userCardTitle == undefined ? "组织用户" : userCardTitle}
                            extra={userCardTitle == undefined ? "" : <Button type="primary" onClick={() => this.addMemberModalVisible(true)}>添加用户</Button>}>
                            <AddMember
                                addMemberModalVisible={this.addMemberModalVisible}
                                modalVisible={adduserModalVisible}
                                findUsersLoading={findUsersLoading}
                                tableFindUsers={tableParameters}
                            />
                            {userCardTitle == undefined ? "选择一个组织" :
                                <Table
                                    rowKey={record => record.id}
                                    size={'middle'}
                                    columns={columns}
                                    pagination={{ pageSize: 10, total: organizationUnitUsersData == undefined ? 0 : organizationUnitUsersData.totalCount, defaultCurrent: 1 }}
                                    loading={organizationUnitUsersData == undefined ? true : false}
                                    dataSource={organizationUnitUsersData == undefined ? [] : organizationUnitUsersData.items}
                                    onChange={this.handleTableChange}
                                    loading={organizationUnitUsersLoading}
                                />
                            }
                        </Card>
                    </Col>
                </Row>
            </PageHeaderWrapper>
        );
    }
}

export default OrganizationUnitsList;
