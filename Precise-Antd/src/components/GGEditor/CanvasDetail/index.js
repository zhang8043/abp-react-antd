import React from 'react';
import { Card, Form, Input,Button } from 'antd';
import { withPropsAPI } from 'gg-editor';

const { Item } = Form;

const inlineFormItemLayout = {
    labelCol: {
        sm: { span: 6 },
    },
    wrapperCol: {
        sm: { span: 18 },
    },
};

class CanvasDetail extends React.Component {

    handleSubmit = (e) => {
        e.preventDefault();

        const { form, propsAPI } = this.props;
        const { getSelected, executeCommand, update } = propsAPI;

        form.validateFieldsAndScroll((err, values) => {
            if (err) {
                return;
            }

            const item = getSelected()[0];

            if (!item) {
                return;
            }

            executeCommand(() => {
                update(item, {
                    ...values,
                });
            });
        });
    }

    handleClick = () => {
        const { propsAPI } = this.props;
        console.log(propsAPI.save());
    };

    render() {
        const { form, propsAPI } = this.props;
        const { getFieldDecorator } = form;
        const { getSelected } = propsAPI;

        return (
            <Card type="inner" title="流程属性" bordered={false}>
                <Form onSubmit={this.handleSubmit}>
                    <Item
                        label="名称"
                        {...inlineFormItemLayout}
                    >
                        {
                            getFieldDecorator('label', {
                                initialValue: "新建流程",
                            })(<Input onBlur={this.handleSubmit} />)
                        }
                    </Item>
                </Form>
                <Button type="primary" size='small' icon='save' onClick={this.handleClick}>保存</Button>
            </Card>
        );
    }
}

export default Form.create()(withPropsAPI(CanvasDetail));
