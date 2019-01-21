import React, { Component } from 'react';
import {
    Card, Col, Row, Button, Divider, Table, Tag, List, Dropdown, Menu, Form, Modal,
    DatePicker,
    InputNumber,
    Input,
    Select,
    Icon,
    Tooltip,
    message,
    Drawer
} from 'antd';
import { connect } from 'dva';
import moment from 'moment';
import { truncateStringWithPostfix } from '@/utils/utils';
import PageHeaderWrapper from '@/components/PageHeaderWrapper';
import styles from './index.less';
import { tuple } from 'antd/lib/_util/type';

const FormItem = Form.Item;
const { RangePicker } = DatePicker;
const dateRanges = [moment().subtract(7, "day").format('YYYY-MM-D h:mm:ss.sssssss'), moment().format('YYYY-MM-D h:mm:ss.sssssss')];

const DescriptionItem = ({ title, content }) => (
    <div
        style={{
            fontSize: 14,
            lineHeight: '22px',
            marginBottom: 7,
            color: 'rgba(0,0,0,0.65)',
        }}
    >
        <p
            style={{
                marginRight: 8,
                display: 'inline-block',
                color: 'rgba(0,0,0,0.85)',
            }}
        >
            {title}:
      </p>
        {content}
    </div>
);

const pStyle = {
    fontSize: 16,
    color: 'rgba(0,0,0,0.85)',
    lineHeight: '24px',
    display: 'block',
    marginBottom: 16,
};
@Form.create()
@connect(({ auditlog, loading }) => ({
    auditlog,
    listLoading: loading.effects['auditlog/getAuditLogs'],
}))
class AuditLogList extends Component {
    state = {
        formValues: {
            StartDate: dateRanges[0],
            EndDate: dateRanges[1],
            MaxResultCount: 6,
            SkipCount: 0,
        },
        expandForm: false,
        visible: false,
        auditLogViews: null
    };

    componentDidMount() {
        this.getAuditLogs();
    }

    getAuditLogs() {
        const { dispatch } = this.props;
        dispatch({
            type: 'auditlog/getAuditLogs',
            payload: this.state.formValues,
        });
    }

    handleTableChange = (pagination) => {
        const { formValues } = this.state;
        formValues.SkipCount = (pagination.current - 1) * this.state.formValues.MaxResultCount;
        this.setState({ formValues: formValues }, this.getAuditLogs());
    };

    handleSearch = e => {
        e.preventDefault();
        const { dispatch, form } = this.props;
        const { formValues } = this.state;
        form.validateFields((err, fieldsValue) => {
            if (err) return;
            formValues.StartDate = moment(fieldsValue.DateRange[0]).format('YYYY-MM-D h:mm:ss.sssssss');
            formValues.EndDate = moment(fieldsValue.DateRange[1]).format('YYYY-MM-D h:mm:ss.sssssss');
            formValues.ServiceName = fieldsValue.ServiceName;
            formValues.UserName = fieldsValue.UserName;
            formValues.MinExecutionDuration = fieldsValue.MinExecutionDuration;
            formValues.MaxExecutionDuration = fieldsValue.MaxExecutionDuration;
            formValues.MethodName = fieldsValue.MethodName;
            formValues.HasException = fieldsValue.HasException;
            this.setState({ formValues: formValues }, this.getAuditLogs());
        });
    };


    handleFormReset = () => {
        const { form, dispatch } = this.props;
        form.resetFields();
        this.setState({
            formValues: {
                StartDate: dateRanges[0],
                EndDate: dateRanges[1],
                MaxResultCount: 6,
                SkipCount: 0,
            },
        }, this.getAuditLogs());
    };

    toggleForm = () => {
        const { expandForm } = this.state.formValues;
        this.setState({
            expandForm: !expandForm,
        });
    };

    renderSimpleForm() {
        const {
            form: { getFieldDecorator },
        } = this.props;
        return (
            <Form onSubmit={this.handleSearch} layout="inline">
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={8} sm={24}>
                        <FormItem label="日期范围">
                            {getFieldDecorator('DateRange', { initialValue: [moment(dateRanges[0], 'YYYY/MM/DD'), moment(dateRanges[1], 'YYYY/MM/DD')] })(<RangePicker />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <FormItem label="用户名">
                            {getFieldDecorator('UserName')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <span className={styles.submitButtons}>
                            <Button type="primary" htmlType="submit">查询</Button>
                            <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>重置</Button>
                            <a style={{ marginLeft: 8 }} onClick={this.toggleForm}>展开 <Icon type="down" /></a>
                        </span>
                    </Col>
                </Row>
            </Form>
        );
    }

    renderAdvancedForm() {
        const {
            form: { getFieldDecorator },
        } = this.props;
        return (
            <Form onSubmit={this.handleSearch} layout="inline">
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={8} sm={24}>
                        <FormItem label="日期范围">
                            {getFieldDecorator('DateRange')(<RangePicker />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <FormItem label="用户名">
                            {getFieldDecorator('UserName')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <FormItem label="服务">
                            {getFieldDecorator('ServiceName')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                </Row>
                <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
                    <Col md={8} sm={24}>
                        <FormItem label="持续时间">
                            {getFieldDecorator('MinExecutionDuration')(<InputNumber style={{ width: '45%' }} />)}
                            <span style={{ width: '10%' }}>---</span>
                            {getFieldDecorator('MaxExecutionDuration')(<InputNumber style={{ width: '45%' }} />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <FormItem label="操作">
                            {getFieldDecorator('MethodName')(<Input placeholder="请输入" />)}
                        </FormItem>
                    </Col>
                    <Col md={8} sm={24}>
                        <FormItem label="错误状态">
                            {getFieldDecorator('HasException')(
                                <Select placeholder="请选择" style={{ width: '100%' }}>
                                    <Option value="">全部</Option>
                                    <Option value="false">成功</Option>
                                    <Option value="true">出现错误</Option>
                                </Select>
                            )}
                        </FormItem>
                    </Col>
                </Row>
                <div style={{ overflow: 'hidden' }}>
                    <div style={{ float: 'right', marginBottom: 24 }}>
                        <Button type="primary" htmlType="submit">
                            查询
                </Button>
                        <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>
                            重置
                </Button>
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

    getFormattedParameters(parameters) {
        const self = this;
        try {
            const json = JSON.parse(parameters);
            return JSON.stringify(json, null, 4);
        } catch (e) {
            return parameters;
        }
    }

    auditLogView() {
        const { auditLogViews } = this.state;
        return (
            <Drawer
                width={640}
                placement="right"
                closable={false}
                onClose={this.onClose}
                visible={this.state.visible}
            >
                {auditLogViews == null ? "" :
                    <div>
                        <p style={{ ...pStyle, marginBottom: 24 }}>
                            {
                                auditLogViews.exception ?
                                    <Icon type="close-circle" theme="twoTone" twoToneColor="#F5222D" />
                                    :
                                    <Icon type="check-circle" theme="twoTone" twoToneColor="#1890FF" />
                            }
                            审计日志详情</p>
                        <Divider />
                        <p style={pStyle}>用户信息</p>
                        <Row>
                            <Col span={8}>
                                <DescriptionItem title="用户名" content={auditLogViews.userName} />{' '}
                            </Col>
                            <Col span={8}>
                                <DescriptionItem title="客户端" content={auditLogViews.clientName} />
                            </Col>
                            <Col span={8}>
                                <DescriptionItem title="IP地址" content={auditLogViews.clientIpAddress} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={24}>
                                <DescriptionItem title="浏览器" content={auditLogViews.browserInfo} />
                            </Col>
                        </Row>
                        <Divider />
                        <p style={pStyle}>操作信息</p>
                        <Row>
                            <Col span={12}>
                                <DescriptionItem title="服务" content={auditLogViews.serviceName} />
                            </Col>
                            <Col span={12}>
                                <DescriptionItem title="操作" content={auditLogViews.methodName} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={12}>
                                <DescriptionItem title="时间" content={moment(auditLogViews.serviceName).format('YYYY年MM月D日 h:mm:ss')} />
                            </Col>
                            <Col span={12}>
                                <DescriptionItem title="持续时间" content={auditLogViews.executionDuration + "ms"} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={24}>
                                <DescriptionItem title="参数" content={
                                    <Card>
                                        <p>{this.getFormattedParameters(auditLogViews.parameters)}</p>
                                    </Card>
                                } />
                            </Col>
                        </Row>
                        <Divider />
                        <p style={pStyle}>自定义数据</p>
                        <Row>
                            <Col span={24}>
                                <Card>
                                    <p>{auditLogViews.customData == null ? "无" : auditLogViews.customData}</p>
                                </Card>
                            </Col>
                        </Row>
                    </div>
                }
            </Drawer>
        );
    }

    showDrawer = (value) => {
        this.setState({
            visible: true,
            auditLogViews: value
        });
    }

    onClose = () => {
        this.setState({
            visible: false,
            auditLogViews: null
        });
    }

    render() {
        const columns = [
            {
                title: '查看', key: 'action',
                render: (value) => (
                    <Button type="primary" shape="circle" icon="search" onClick={() => this.showDrawer(value)} />
                ),
            },
            {
                title: '', dataIndex: 'exception', key: 'exception',
                render: (value, row, index) => {
                    if (value)
                        return <Icon type="close-circle" theme="twoTone" twoToneColor="#F5222D" />
                    else
                        return <Icon type="check-circle" theme="twoTone" twoToneColor="#1890FF" />
                },
            },
            {
                title: '时间', dataIndex: 'executionTime', key: 'executionTime',
                render: (value, row, index) => {
                    return moment(value).format('YYYY年MM月D日 h:mm:ss')
                },
            },
            { title: '用户名', dataIndex: 'userName', key: 'userName', },
            { title: '服务', dataIndex: 'serviceName', key: 'serviceName', },
            { title: '操作', dataIndex: 'methodName', key: 'methodName', },
            {
                title: '持续时间', dataIndex: 'executionDuration', key: 'executionDuration',
                render: (value, row, index) => {
                    return value + "ms";
                },
            },
            { title: 'IP地址', dataIndex: 'clientIpAddress', key: 'clientIpAddress', },
            {
                title: '浏览器', dataIndex: 'browserInfo', key: 'browserInfo',
                render: (value, row, index) => {
                    return (
                        <Tooltip title={value}>
                            {truncateStringWithPostfix(value, 20)}
                        </Tooltip>);
                },
            },
        ];

        const {
            listLoading,
            auditlog: { data },
        } = this.props;

        return (
            <PageHeaderWrapper title="审计日志">
                <Card bordered={false}>
                    <div className={styles.tableList}>
                        <div className={styles.tableListForm}>{this.renderForm()}</div>
                        <div className={styles.tableListOperator}>

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
                    {this.auditLogView()}
                </Card>
            </PageHeaderWrapper>
        );
    }
}

export default AuditLogList;