import React from 'react'
import { Dropdown, Button, Icon, Menu } from 'antd'

const DropOptionButton = ({
    onMenuClick,
    menuOptions = [],
    buttonText,
    buttonStyle,
    dropdownProps,
}) => {
    const menu = menuOptions.map(item => (
        <Menu.Item key={item.key}>{item.name}</Menu.Item>
    ))
    return (
        <Dropdown
            overlay={<Menu onClick={onMenuClick}>{menu}</Menu>}
            {...dropdownProps}
        >
            <Button style={{ ...buttonStyle }}>
                {buttonText ? buttonText :
                    <Icon style={{ marginRight: 2 }} type="bars" />
                }
                <Icon type="down" />
            </Button>
        </Dropdown>
    )
}

export default DropOptionButton
