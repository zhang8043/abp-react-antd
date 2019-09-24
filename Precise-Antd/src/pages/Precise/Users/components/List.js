import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Table, message, Modal, Avatar, Tag, Button, Tooltip, Divider, Switch } from 'antd';
import moment from 'moment';
import DropOption from '@/components/DropOption';
import router from 'umi/router';
import StandardTable from '@/components/StandardTable';

const { confirm } = Modal;

class List extends PureComponent {

    dropOptionClick = (record, e) => {
        const { onDeleteItem, onEditItem, onUseUserLogin, onPermission, onUnlock } = this.props;
        if (e.key === '0') {
            onUseUserLogin(record.id);
        }
        else if (e.key === '1') {
            onEditItem(record.id);
        }
        else if (e.key === '2') {
            onPermission(record.id);
        }
        else if (e.key === '3') {
            onUnlock(record.id);
        }
        else if (e.key === '4') {
            confirm({
                title: "您确定删除此记录吗？",
                onOk() {
                    onDeleteItem(record.id);
                }
            })
        }
    }

    render() {
        const { onDeleteItem, onEditItem, onUseUserLogin, onPermission, onUnlock, onSwitchChange, ...listProps } = this.props;

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
                            onMenuClick={e => this.dropOptionClick(row, e)}
                            menuOptions={[
                                { key: '0', name: `使用这个用户登录` },
                                { key: '1', name: `修改` },
                                { key: '2', name: `权限` },
                                { key: '3', name: `解锁` },
                                { key: '4', name: `删除` },
                            ]}
                        />
                    );
                },
            }];

        return (
            <StandardTable
                {...listProps}
                columns={columns}
                size={'middle'}
                rowKey={record => record.id}
            />
        );
    }
}

List.propTypes = {
    onDeleteItem: PropTypes.func,
    onEditItem: PropTypes.func,
    onUseUserLogin: PropTypes.func,
    onPermission: PropTypes.func,
    onUnlock: PropTypes.func,
    onSwitchChange: PropTypes.func,
    location: PropTypes.object,
}

export default List;