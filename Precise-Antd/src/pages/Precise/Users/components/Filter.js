import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import { Form, Button, Row, Col, DatePicker, Input, Cascader, Select, Steps, Divider, Radio, Icon, InputNumber, Checkbox } from 'antd';
import styles from '../index.less';

const FormItem = Form.Item;
const { Step } = Steps;
const { TextArea } = Input;
const { Option } = Select;
const RadioGroup = Radio.Group;
const Search = Input.Search;

@Form.create()
class Filter extends PureComponent {

    state = {
        expandForm: false,
    };

    handleSearch = e => {
        const { form, onFilterChange } = this.props;
        form.validateFields((err, fieldsValue) => {
            if (err) return;
            const values = {
                ...fieldsValue
            };
            onFilterChange(values);
        });
    };

    handleFormReset = () => {
        const { form, refresh } = this.props;
        form.resetFields();
        refresh();
    };

    render() {
        const { form: { getFieldDecorator }, userSelectedRowKeys } = this.props;
        const { expandForm } = this.state;

        const toggleForm = () => {
            this.setState({
                expandForm: !expandForm,
            });
        };

        const renderForm = () => {
            return expandForm ? renderAdvancedForm() : renderSimpleForm();
        }

        const renderSimpleForm = () => {
            return (
                <div>
                    {getFieldDecorator('Filter')(
                        <Search
                            placeholder="搜索"
                            onSearch={this.handleSearch}
                            enterButton
                        />
                    )}
                    <div style={{ marginTop: 12 }}>
                        <Row>
                            <Col span={24} style={{ textAlign: 'left' }}>
                                <a style={{ marginLeft: 8 }} onClick={toggleForm}>
                                    显示高级过滤 <Icon type="down" />
                                </a>
                            </Col>
                        </Row>
                    </div>
                </div>
            );
        }

        const renderAdvancedForm = () => {
            return (
                <div>
                    <Row>
                        <Col>
                            <FormItem>
                                {getFieldDecorator('Filter')(
                                    <Search
                                        placeholder="搜索"
                                        onSearch={this.handleSearch}
                                        enterButton
                                    />
                                )}
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={{ md: 12, lg: 24, xl: 48 }}>
                        <Col md={12} sm={24}>
                            <FormItem>
                                {getFieldDecorator('Permission')(
                                    <Select placeholder="按许可搜索" style={{ width: '100%' }}>
                                        <Option value="0">关闭</Option>
                                        <Option value="1">运行中</Option>
                                    </Select>
                                )}
                            </FormItem>
                        </Col>
                        <Col md={12} sm={24}>
                            <FormItem>
                                {getFieldDecorator('Role')(
                                    <Select placeholder="按角色搜索" style={{ width: '100%' }}>
                                        <Option value="0">关闭</Option>
                                        <Option value="1">运行中</Option>
                                    </Select>
                                )}
                            </FormItem>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <FormItem>
                                {getFieldDecorator('OnlyLockedUsers')(
                                    <Checkbox>仅已锁定用户</Checkbox>
                                )}
                            </FormItem>
                        </Col>
                    </Row>
                    <Row>
                        <Col span={12} style={{ textAlign: 'left' }}>
                            <a style={{ marginLeft: 8 }} onClick={toggleForm}>
                                显示高级过滤 <Icon type="up" />
                            </a>
                        </Col>
                        <Col span={12} style={{ textAlign: 'right' }}>
                            <Button type="primary" icon="reload" onClick={this.handleFormReset}>刷新</Button>
                        </Col>
                    </Row>
                </div>
            );
        }

        return (
            <div>
                <div className={styles.tableListForm}>{renderForm()}</div>
                <div className={styles.tableListOperator}>
                    {userSelectedRowKeys.length > 0 && (
                        <span>
                            <Button>批量操作</Button>
                        </span>
                    )}
                </div>
            </div>
        )
    }
}

Filter.propTypes = {

}

export default Filter
