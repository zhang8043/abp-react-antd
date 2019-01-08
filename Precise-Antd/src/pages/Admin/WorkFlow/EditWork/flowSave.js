import React from "react";
import { withPropsAPI } from "gg-editor";
import { Button } from 'antd';

class FlowSave extends React.Component {
  handleClick = () => {
    const { propsAPI } = this.props;
    console.log(propsAPI.save());
  };

  render() {
    return (
      <Button type="primary" size='small' icon='save' onClick={this.handleClick}>保存</Button>
    );
  }
}

export default withPropsAPI(FlowSave);
