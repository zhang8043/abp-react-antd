import React, { Component } from 'react';
import { Card, Col, Row, Button, Divider, Table, Tag, Dropdown, Menu, Modal } from 'antd';
import { connect } from 'dva';
import moment from 'moment';

@connect(({ auditlog }) => ({
    auditlog,
}))
class AuditLogList extends Component {
    state = {
        StartDate: "2019-01-07T16%3A00%3A00.000Z",
        EndDate: "2019-01-08T15%3A59%3A59.999Z",
        MaxResultCount: 100,
        SkipCount: 0
    };

    componentDidMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'auditlog/getAuditLogs',
            payload: this.state,
        });
    }

    render() {
        const columns = [
            { title: '时间', dataIndex: 'executionTime', key: 'executionTime', },
            { title: '用户名', dataIndex: 'userName', key: 'userName', },
            { title: '服务', dataIndex: 'Service', key: 'Service', },
            { title: '操作', dataIndex: 'Action', key: 'Action', },
            { title: '持续时间', dataIndex: 'executionDuration', key: 'executionDuration', },
            { title: 'IP地址', dataIndex: 'IpAddress', key: 'IpAddress', },
            {
                title: '操作', key: 'action',
                render: () => (
                    <span>
                        <a href="javascript:;">查看</a>
                    </span>
                ),
            }];

        const {
            auditlog: { data },
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

export default AuditLogList;