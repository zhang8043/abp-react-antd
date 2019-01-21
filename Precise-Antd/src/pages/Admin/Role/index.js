import React, { Component } from 'react';
import { connect } from 'dva';
import {
    Card, Col, Row, Button, Divider, Table, Tag, Menu, Form, Modal, DatePicker, Input,
    Select,
    Dropdown,
    Icon,
    Tooltip,
    message,
    InputNumber,
    TreeSelect,
} from 'antd';
import PageHeaderWrapper from '@/components/PageHeaderWrapper';
import styles from './index.less';
import moment from 'moment';
import DropOption from '@/components/DropOption';

const FormItem = Form.Item;

@Form.create()
@connect(({ loading, role, permission }) => ({
    listLoading: loading.effects['role/getRoles'],
    role,
    permission,
}))
class RoleList extends Component {
    state = {
        Permission: "",
    };

    componentDidMount() {
        this.getRoles();

        const { dispatch } = this.props;
        dispatch({
            type: 'permission/getAllPermissions',
        });
    }

    getRoles() {
        const { dispatch } = this.props;
        dispatch({
            type: 'role/getRoles',
            payload: this.state.Permission,
        });
    }

    handleFormReset = () => {
        const { form, dispatch } = this.props;
        form.resetFields();
        this.setState({ Permission: "" }, this.getRoles());
    };

    handleSearch = e => {
        e.preventDefault();
        const { dispatch, form } = this.props;
        form.validateFields((err, fieldsValue) => {
            if (err) return;
            dispatch({
                type: 'role/getRoles',
                payload: fieldsValue.Permission,
            });
        });
    };

    jsonDataTree(data, parent) {
        var itemArr = [];
        for (var i = 0; i < data.length; i++) {
            var node = data[i];
            if (node.parentName == parent) {
                var newNodes = { key: node.name, value: node.name, title: node.displayName, children: this.jsonDataTree(data, node.name) };
                itemArr.push(newNodes);
            }
        }
        return itemArr;
    }

    renderSimpleForm() {
        const {
            form: { getFieldDecorator },
            permission: { allPermissions },
        } = this.props;

        const allPermissionsData = this.jsonDataTree(allPermissions, null);

        return (
            <Form onSubmit={this.handleSearch} layout="inline">
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={20} sm={24}>
                        <FormItem label="权限">
                            {getFieldDecorator('Permission', { initialValue: this.Permission })(
                                <TreeSelect
                                    style={{ width: "100%" }}
                                    dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                                    treeData={allPermissionsData}
                                    placeholder="按权限搜索"
                                    treeDefaultExpandAll
                                />)}
                        </FormItem>
                    </Col>
                    <Col md={4} sm={24}>
                        <span className={styles.submitButtons}>
                            <Button type="primary" htmlType="submit">查询</Button>
                            <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>重置</Button>
                        </span>
                    </Col>
                </Row>
            </Form>
        );
    }

    handleMenuClick(record, e) {
        if (e.key === '1') {
            console.log("修改" + e.key);
        }
    }

    render() {
        const columns = [
            { title: '角色名称', dataIndex: 'displayName', key: 'displayName', },
            {
                title: '系统角色', dataIndex: 'isStatic', key: 'isStatic',
                render: (value, row, index) => {
                    if (value)
                        return <Tag color="#2db7f5">是</Tag>;
                    else
                        return <Tag color="#f50">否</Tag>;
                },
            },
            {
                title: '是否默认', dataIndex: 'isDefault', key: 'isDefault',
                render: (value, row, index) => {
                    if (value)
                        return <Tag color="#2db7f5">是</Tag>;
                    else
                        return <Tag color="#f50">否</Tag>;
                },
            },
            {
                title: '创建时间', dataIndex: 'creationTime', key: 'creationTime',
                render: (value, row, index) => {
                    return moment(value).format('YYYY-MM-D h:mm:ss');
                },
            },
            {
                title: '操作', key: 'action',
                render: (value, row, index) => {
                    return (
                        <DropOption
                            onMenuClick={e => this.handleMenuClick(row, e)}
                            menuOptions={[
                                { key: '1', name: `修改` },
                            ]}
                        />
                    );
                },
            }];

        const {
            listLoading,
            role: { data },
        } = this.props;

        return (
            <PageHeaderWrapper title="角色管理">
                <Card bordered={false}>
                    <div className={styles.tableList}>
                        <div className={styles.tableListForm}>{this.renderSimpleForm()}</div>
                        <div className={styles.tableListOperator}>
                            12
                        </div>
                        <Table
                            rowKey={record => record.id}
                            size={'middle'}
                            columns={columns}
                            pagination={{ pageSize: 10, total: data == undefined ? 0 : data.totalCount, defaultCurrent: 1 }}
                            loading={data == undefined ? true : false}
                            dataSource={data == undefined ? [] : data.items}
                            loading={listLoading}
                        />
                    </div>
                </Card>
            </PageHeaderWrapper>
        );
    }
}

export default RoleList;
