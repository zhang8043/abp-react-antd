import React from 'react';
import { Card } from 'antd';
import {
  NodePanel,
  EdgePanel,
  GroupPanel,
  MultiPanel,
  CanvasPanel,
  DetailPanel,
} from 'gg-editor';
import NodeDetail from '../NodeDetail';
import EdgeDetail from '../EdgeDetail';
import GroupDetail from '../GroupDetail';
import CanvasDetail from '../CanvasDetail';
import styles from './index.less';

class FlowDetailPanel extends React.Component {
  render() {
    return (
      <DetailPanel className={styles.detailPanel}>
        <NodePanel>
          {/* 节点属性 */}
          <NodeDetail />
        </NodePanel>
        <EdgePanel>
          {/* 连接线 */}
          <EdgeDetail />
        </EdgePanel>
        <GroupPanel>
          {/* 群组属性 */}
          <GroupDetail />
        </GroupPanel>
        <MultiPanel>
          <Card type="inner" title="多选属性" bordered={false} />
        </MultiPanel>
        <CanvasPanel>
          <CanvasDetail />
          {/* <Card type="inner" title="流程属性" bordered={false} /> */}
        </CanvasPanel>
      </DetailPanel>
    );
  }
}

export default FlowDetailPanel;
