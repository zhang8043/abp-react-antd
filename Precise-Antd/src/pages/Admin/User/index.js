import React, { PureComponent } from 'react';
import { connect } from 'dva';
import { Card, Col, Row, Button, Divider, Table, Tag, Dropdown, Menu, Modal } from 'antd';

@connect(({ user }) => ({
    user,
}))
class UserList extends PureComponent {
    state = {
        Filter:"",
        maxResultCount: 100,
        SkipCount: 0
    };

    componentDidMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'user/getUsers',
            payload: this.state,
        });
    }

    render() {
        const columns = [
            { title: '用户名', dataIndex: 'username', key: 'username', },
            { title: '姓名', dataIndex: 'name', key: 'name', },
            { title: '角色', dataIndex: 'Roles', key: 'Roles', },
            { title: '邮箱地址', dataIndex: 'emailAddress', key: 'emailAddress', },
            { title: '邮箱地址验证', dataIndex: 'isEmailConfirmed', key: 'isEmailConfirmed', },
            { title: '激活', dataIndex: 'isActive', key: 'isActive', },
            { title: '上次登录时间', dataIndex: 'lastLoginTime', key: 'lastLoginTime', },
            { title: '创建时间', dataIndex: 'creationTime', key: 'creationTime', },
            {
                title: 'Action', key: 'action',
                render: () => (
                    <span>
                        <a href="javascript:;">修改</a>
                        <Divider type="vertical" />
                        <a href="javascript:;">删除</a>
                    </span>
                ),
            }];

        const {
            user: { data },
        } = this.props;

        return (
            <Card>
                <Row>
                    <Col>
                        <Table
                            rowKey={record => record.id}
                            size={'middle'}
                            columns={columns}
                            pagination={{ pageSize: 10, total: data == undefined ? 0 : data.totalCount, defaultCurrent: 1 }}
                            loading={data == undefined ? true : false}
                            dataSource={data == undefined ? [] : data.items}
                        />
                    </Col>
                </Row>
            </Card>
        );
    }
}

export default UserList;
