import React, { Component, PureComponent } from 'react';
import { connect } from 'dva';
import { Modal, Table, message } from 'antd';

@connect(({ organization }) => ({
    organization,
}))
class AddMember extends Component {

    state = {
        selectedRowKeys: [],
    };

    onSelectChange = (selectedRowKeys) => {
        this.setState({ selectedRowKeys });
    }

    render() {
        const { modalVisible, addMemberModalVisible, organization: { findUsersData }, findUsersLoading, tableFindUsers } = this.props;
        const { selectedRowKeys } = this.state;

        const columns = [
            { title: '姓名', dataIndex: 'name', key: 'name', },
        ];

        const rowSelection = {
            selectedRowKeys,
            onChange: this.onSelectChange,
            getCheckboxProps: record => ({
                disabled: record.value === '2',
            }),
        };

        const okHandle = () => {
            this.props.dispatch({
                type: 'organization/addUsersToOrganizationUnit',
                payload: { userIds: selectedRowKeys, organizationUnitId: tableFindUsers.id },
                callback: (res) => {
                    if (res) {
                        message.success('操作成功');
                        this.props.dispatch({
                            type: 'organization/getOrganizationUnitUsers',
                            payload: tableFindUsers
                        });
                    }
                }
            });
            addMemberModalVisible();
        };

        return (
            <Modal
                destroyOnClose
                title="选择用户"
                okText="保存"
                destroyOnClose={true}
                visible={modalVisible}
                onOk={okHandle}
                onCancel={() => addMemberModalVisible()}
            >
                <Table
                    rowKey={record => record.value}
                    rowSelection={rowSelection}
                    size={'small'}
                    columns={columns}
                    pagination={{ pageSize: 10, total: findUsersData == undefined ? 0 : findUsersData.totalCount, defaultCurrent: 1 }}
                    loading={findUsersData == undefined ? true : false}
                    dataSource={findUsersData == undefined ? [] : findUsersData.items}
                    onChange={this.handleTableChange}
                    loading={findUsersLoading}
                />
            </Modal>
        );
    }
}

export default AddMember;
