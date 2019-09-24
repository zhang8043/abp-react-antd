import React, { Component } from 'react';
import { connect } from 'dva';
import router from 'umi/router';
import { stringify } from 'qs';
import { Row, Col, Card, Popconfirm, Button, message, Divider, Modal, Alert, Menu, Icon, Dropdown } from 'antd';
import PageHeaderWrapper from '@/components/PageHeaderWrapper';
import DropOptionButton from '@/components/DropOptionButton';
import IconFont from '@/components/IconFont/index';
import Filter from './components/Filter';
import List from './components/List';
import styles from './index.less';

const ButtonGroup = Button.Group;

@connect(({ users, loading }) => ({
    users, loading,
}))
class Cars extends Component {

    render() {
        const { location, dispatch, users, loading } = this.props;
        const { query, pathname } = location;

        const {
            list,
            pagination,
            userSelectedRowKeys,
        } = users;

        //处理刷新
        const handleRefresh = newQuery => {
            router.replace({
                pathname,
                search: stringify(
                    newQuery,
                    { arrayFormat: 'repeat' }
                ),
            })
        }

        const action = (
            <div>
                <DropOptionButton
                    buttonText="Excel 操作"
                    onMenuClick={e => this.dropOptionClick(row, e)}
                    menuOptions={[
                        { key: '0', name: `导出到 Excel` },
                        { key: '1', name: `从 Excel 导入` },
                        { key: '2', name: `下载导入模板` },
                    ]}
                />
                <Button icon="plus" type="primary">添加用户</Button>
            </div>
        );

        const filterProps = {
            userSelectedRowKeys,
            onFilterChange(value) {
                handleRefresh({
                    ...value,
                    page: 1,
                })
            },
            //刷新
            refresh() {
                handleRefresh(null);
            },
        }

        //列表
        const listProps = {
            loading: loading.effects['users/getLeave'],
            data: { list, pagination: pagination },
            selectedRows: userSelectedRowKeys,
            pagination,
            //分页、排序、筛选
            onChange(page) {
                handleRefresh({
                    skipCount: page.current,
                    maxResultCount: page.pageSize,
                })
            },
            //选中行
            onSelectRow: keys => {
                dispatch({
                    type: 'users/updateState',
                    payload: {
                        userSelectedRowKeys: keys,
                    },
                })
            },
            //删除
            onDeleteItem(id) {
                dispatch({
                    type: 'users/delete',
                    payload: id,
                }).then(() => {
                    handleRefresh({
                        skipCount:
                            list.length === 1 && pagination.current > 1
                                ? pagination.current - 1
                                : pagination.current,
                    })
                })
            },
            //修改
            onEditItem(id) {
                message.success(id);
            },
            //使用这个用户登录
            onUseUserLogin(id) {
                message.success(id);
            },
            //权限
            onPermission(id) {
                message.success(id);
            },
            //解锁
            onUnlock(id) {
                message.success(id);
            },
            //激活？
            onSwitchChange(id) {
                message.success(id);
            }
        }

        return (
            <PageHeaderWrapper
                title="用户"
                content="管理用户及权限."
                action={action}>
                <Card bordered={false}>
                    <div className={styles.tableList}>
                        <Filter {...filterProps} />
                        <List {...listProps} />
                    </div>
                </Card>
            </PageHeaderWrapper >
        );
    }
}

export default Cars;