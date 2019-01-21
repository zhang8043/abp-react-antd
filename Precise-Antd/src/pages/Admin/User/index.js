import React, { PureComponent } from 'react';
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
import DropOption from '@/components/DropOption';
import styles from './index.less';
import moment from 'moment';

const FormItem = Form.Item;

@Form.create()
@connect(({ loading, user, permission, role }) => ({
    user,
    permission,
    role,
    listLoading: loading.effects['user/getUsers'],
}))
class UserList extends PureComponent {
    state = {
        formValues: {
            Filter: "",
            Permission: "",
            Role: "",
            OnlyLockedUsers: false,
            MaxResultCount: 6,
            SkipCount: 0,
        },
        expandForm: false,
    };

    componentDidMount() {
        this.getUsers();

        const { dispatch } = this.props;
        dispatch({
            type: 'permission/getAllPermissions',
        });

        dispatch({
            type: 'role/getRoles',
            permission: null,
        });
    }

    getUsers() {
        const { dispatch } = this.props;
        dispatch({
            type: 'user/getUsers',
            payload: this.state.formValues,
        });
    }

    handleTableChange = (pagination) => {
        const { formValues } = this.state;
        formValues.SkipCount = (pagination.current - 1) * formValues.MaxResultCount;
        this.setState({ formValues: formValues }, this.getAuditLogs());
    };

    toggleForm = () => {
        const { expandForm } = this.state;
        this.setState({
            expandForm: !expandForm,
        });
    };

    handleSearch = e => {
        e.preventDefault();
        const { dispatch, form } = this.props;
        const { formValues } = this.state;
        form.validateFields((err, fieldsValue) => {
            if (err) return;
            formValues.Permission = fieldsValue.Permission;
            formValues.Filter = fieldsValue.Filter;
            formValues.Role = fieldsValue.Role;
            formValues.MaxResultCount = formValues.MaxResultCount;
            formValues.SkipCount = formValues.SkipCount;
            this.setState({ formValues: formValues }, this.getUsers());
        });
    };


    handleFormReset = () => {
        const { form, dispatch } = this.props;
        form.resetFields();
        this.setState({
            formValues: {
                MaxResultCount: 6,
                SkipCount: 0,
            },
        }, this.getUsers());
    };

    renderSimpleForm() {
        const {
            form: { getFieldDecorator },
        } = this.props;
        return (
            <Form onSubmit={this.handleSearch} layout="inline">
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={16} sm={24}>
                        <FormItem label="搜索">
                            {getFieldDecorator('Filter')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <span className={styles.submitButtons}>
                            <Button type="primary" htmlType="submit">查询</Button>
                            <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>重置</Button>
                            <a style={{ marginLeft: 8 }} onClick={this.toggleForm}>
                                展开 <Icon type="down" />
                            </a>
                        </span>
                    </Col>
                </Row>
            </Form>
        );
    }

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

    roleDataTree(data, parent) {
        var itemArr = [];
        for (var i = 0; i < data.length; i++) {
            var node = data[i];
            if (node.parentName == parent) {
                var newNodes = { key: node.name, value: node.id, title: node.displayName, children: this.roleDataTree(data, node.name) };
                itemArr.push(newNodes);
            }
        }
        return itemArr;
    }

    renderAdvancedForm() {
        const {
            form: { getFieldDecorator },
            permission: { allPermissions },
            role: { data },
        } = this.props;

        const allPermissionsData = this.jsonDataTree(allPermissions, null);
        const roleData = this.roleDataTree(data.items, null);

        return (
            <Form onSubmit={this.handleSearch} layout="inline">
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={24} sm={24}>
                        <FormItem label="搜索">
                            {getFieldDecorator('Filter')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                </Row>
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={12} sm={24}>
                        <FormItem label="权限">
                            {getFieldDecorator('Permission', { initialValue: this.state.formValues.Permission })(
                                <TreeSelect
                                    style={{ width: "100%" }}
                                    dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                                    treeData={allPermissionsData}
                                    placeholder="按权限搜索"
                                    treeDefaultExpandAll
                                />)}
                        </FormItem>
                    </Col>
                    <Col md={12} sm={24}>
                        <FormItem label="角色">
                            {getFieldDecorator('Role', { initialValue: this.state.formValues.Role })(
                                <TreeSelect
                                    style={{ width: "100%" }}
                                    dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                                    treeData={roleData}
                                    placeholder="按角色搜索"
                                    treeDefaultExpandAll
                                />)}
                        </FormItem>
                    </Col>
                </Row>
                <div style={{ overflow: 'hidden' }}>
                    <div style={{ float: 'right', marginBottom: 24 }}>
                        <Button type="primary" htmlType="submit">查询</Button>
                        <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>重置</Button>
                        <a style={{ marginLeft: 8 }} onClick={this.toggleForm}>
                            收起 <Icon type="up" />
                        </a>
                    </div>
                </div>
            </Form>
        );
    }

    renderForm() {
        const { expandForm } = this.state;
        return expandForm ? this.renderAdvancedForm() : this.renderSimpleForm();
    }

    handleMenuClick(record, e) {
        if (e.key === '0') {
            message.info(e.item.props.children);
        } else if (e.key === '1') {
            message.info(e.item.props.children);
        } else if (e.key === '2') {
            message.info(e.item.props.children);
        } else if (e.key === '3') {
            message.info(e.item.props.children);
        } else if (e.key === '4') {
            message.info(e.item.props.children);
        }
        console.log(record);
    }

    render() {
        const columns = [
            { title: '姓名', dataIndex: 'name', key: 'name', },
            { title: '用户名', dataIndex: 'userName', key: 'userName', },
            {
                title: '角色', dataIndex: 'roles', key: 'roles',
                render: (value, row, index) => {
                    let roleNames = '';
                    for (let j = 0; j < value.length; j++) {
                        if (roleNames.length) {
                            roleNames = roleNames + ', ';
                        }
                        roleNames = roleNames + value[j].roleName;
                    }
                    return roleNames;
                },
            },
            { title: '邮箱地址', dataIndex: 'emailAddress', key: 'emailAddress', },
            {
                title: '邮箱地址验证', dataIndex: 'isEmailConfirmed', key: 'isEmailConfirmed',
                render: (value, row, index) => {
                    if (value)
                        return <Tag color="#2db7f5">是</Tag>;
                    else
                        return <Tag color="#f50">否</Tag>;
                },
            },
            {
                title: '激活', dataIndex: 'isActive', key: 'isActive',
                render: (value, row, index) => {
                    if (value)
                        return <Tag color="#2db7f5">是</Tag>;
                    else
                        return <Tag color="#f50">否</Tag>;
                },
            },
            {
                title: '上次登录时间', dataIndex: 'lastLoginTime', key: 'lastLoginTime',
                render: (value, row, index) => {
                    return moment(value).format('YYYY-MM-D h:mm:ss');
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
                                { key: '0', name: `使用这个用户登录` },
                                { key: '1', name: `修改` },
                                { key: '2', name: `权限` },
                                { key: '3', name: `修改` },
                                { key: '4', name: `删除` },
                            ]}
                        />
                    );
                },
            }];

        const {
            listLoading,
            user: { data },
            permission: { allPermissions },
        } = this.props;

        return (
            <PageHeaderWrapper title="用户管理">
                <Card bordered={false}>
                    <div className={styles.tableList}>
                        <div className={styles.tableListForm}>{this.renderForm()}</div>
                        <div className={styles.tableListOperator}>
                            <Button icon="plus" type="primary" onClick={() => this.handleModalVisible(true)}>
                                新建 </Button>
                        </div>
                        <Table
                            rowKey={record => record.id}
                            size={'middle'}
                            columns={columns}
                            pagination={{ pageSize: 10, total: data == undefined ? 0 : data.totalCount, defaultCurrent: 1 }}
                            loading={data == undefined ? true : false}
                            dataSource={data == undefined ? [] : data.items}
                            onChange={this.handleTableChange}
                            loading={listLoading}
                        />
                    </div>
                </Card>
            </PageHeaderWrapper>
        );
    }
}

export default UserList;
