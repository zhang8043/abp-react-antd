import React, { Component } from 'react';
import { connect } from 'dva';
import { Tree, Icon } from 'antd';
import { createTree } from '@/utils/utils';

const { TreeNode } = Tree;

@connect(({ loading, organization }) => ({
    organizationLoading: loading.effects['organization/getOrganizationUnits'],
    organization,
}))
class OrganizationUnitsList extends Component {

    state={
        isSu:true
    };

    componentDidMount() {
        this.getOrganizationUnits();
    }

    getOrganizationUnits() {
        const { dispatch } = this.props;
        dispatch({
            type: 'organization/getOrganizationUnits',
        });
    }

    moveOrganizationUnit(id, newParentId) {
        const { dispatch } = this.props;
        dispatch({
            type: 'organization/moveOrganizationUnit',
            payload: { Id: id, NewParentId: newParentId },
        });
    }

    onDrop = (info) => {
        const { organization: { organizationData } } = this.props;
        const dropKey = info.node.props.eventKey;
        const dragKey = info.dragNode.props.eventKey;
        const dropPos = info.node.props.pos.split('-');
        const dropPosition = info.dropPosition - Number(dropPos[dropPos.length - 1]);

        const loop = (data, key, callback) => {
            data.forEach((item, index, arr) => {
                if (item.key === key) {
                    return callback(item, index, arr);
                }
                if (item.children) {
                    return loop(item.children, key, callback);
                }
            });
        };
        const data = [...organizationData];

        let dragObj;
        loop(data, dragKey, (item, index, arr) => {
            arr.splice(index, 1);
            dragObj = item;
        });

        loop(data, dropKey, (item) => {
            item.children = item.children || [];
            console.log(dragObj.key + "移动到" + item.key);
            this.moveOrganizationUnit(dragObj.key, item.key)
        });
        this.getOrganizationUnits();
        this.setState({isSu:true});
    }



    render() {
        const loop = data => data.map((item) => {
            if (item.children && item.children.length) {
                return <TreeNode key={item.key} title={item.displayName}>{loop(item.children)}</TreeNode>;
            }
            return <TreeNode key={item.key} title={item.displayName} />;
        });

        const {
            organizationLoading,
            organization: { organizationData },
        } = this.props;

        return (
            <Tree
                className="draggable-tree"
                draggable
                onDrop={this.onDrop}
            >
                {loop(organizationData)}
            </Tree>
        );
    }
}

export default OrganizationUnitsList;
