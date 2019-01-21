import React, { Component, PureComponent } from 'react';
import { connect } from 'dva';
import { Tree, Icon, notification, Modal, Button, Row, Col, Card, Form, Input, message, Menu } from 'antd';

const FormItem = Form.Item;
@Form.create()
@connect(({ organization }) => ({
  organization,
}))
export default class CreateOrUpdateForm extends PureComponent {

  handleAdd = fields => {
    const { dispatch } = this.props;
    dispatch({
      type: 'organization/createOrganizationUnit',
      payload: { displayName: fields.displayName },
    });
  };

  handleChildAdd = fields => {
    const { dispatch } = this.props;
    dispatch({
      type: 'organization/createOrganizationUnit',
      payload: fields,
    });
  };

  handleUpdate = fields => {
    const { dispatch } = this.props;
    dispatch({
      type: 'organization/updateOrganizationUnit',
      payload: fields,
    });
  };

  render() {

    const { modalType, modalVisible, handleModalVisible, formVals, form, handleAdd, handleUpdate } = this.props;

    const okHandle = () => {
      form.validateFields((err, fieldsValue) => {
        if (err) return;
        form.resetFields();
        if (modalType === null)
          this.handleAdd(fieldsValue);
        else if (modalType === "add")
          this.handleChildAdd(fieldsValue);
        else if (modalType === "update")
          this.handleUpdate(fieldsValue);

        handleModalVisible()
      });
    };

    return (
      <Modal
        destroyOnClose
        title={modalType == "update" ? "修改: " + formVals.displayName : "新建组织机构"}
        okText="保存"
        visible={modalVisible}
        onOk={okHandle}
        onCancel={() => handleModalVisible()}
      >
        <FormItem key="id" style={{ display: "none" }}>
          {form.getFieldDecorator('id', {
            initialValue: formVals.id,
          })(<Input />)}
        </FormItem>
        <FormItem key="displayName" labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="名称">
          {form.getFieldDecorator('displayName', {
            rules: [{ required: true, message: '请输入组织机的名称！' }],
            initialValue: modalType == "add" ? "" : formVals.displayName,
          })(<Input placeholder="请输入" />)}
        </FormItem>
        <FormItem key="parentId" style={{ display: "none" }}>
          {form.getFieldDecorator('parentId', {
            initialValue: formVals.parentId,
          })(<Input />)}
        </FormItem>
      </Modal>
    );
  }
}
