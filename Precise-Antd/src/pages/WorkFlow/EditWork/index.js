import React, { Component } from 'react';
import { Row, Col, Button } from 'antd';
import GGEditor, { Flow } from 'gg-editor';
import EditorMinimap from '@/components/GGEditor/EditorMinimap';
import { FlowContextMenu } from '@/components/GGEditor/EditorContextMenu';
import { FlowToolbar } from '@/components/GGEditor/EditorToolbar';
import { FlowItemPanel } from '@/components/GGEditor/EditorItemPanel';
import { FlowDetailPanel } from '@/components/GGEditor/EditorDetailPanel';
import styles from './index.less';

class EditWork extends React.Component {
    renderFlow() {
        return (
            <Flow className={styles.flow} />
        );
    }

    render() {
        return (
            <GGEditor className={styles.editor}>
                <Row type="flex" className={styles.editorHd}>
                    <Col span={24}>
                        <FlowToolbar />
                    </Col>
                </Row>
                <Row type="flex" className={styles.editorBd}>
                    <Col span={4} className={styles.editorSidebar}>
                        <FlowItemPanel />
                    </Col>
                    <Col span={16} className={styles.editorContent}>
                        {this.renderFlow()}
                    </Col>
                    <Col span={4} className={styles.editorSidebar}>
                        <FlowDetailPanel />
                        <EditorMinimap />
                    </Col>
                </Row>
                <FlowContextMenu />
            </GGEditor>
        );
    }
}

export default EditWork;
