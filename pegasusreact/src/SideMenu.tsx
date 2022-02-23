import React from 'react';
import { MenuApiModel } from './ApiModels'
import MenuItem from './MenuItem';

export default class SideMenu extends React.Component<MenuApiModel, any> {
    public constructor(props: MenuApiModel) {
        super(props)
    }

    public renderMenu(): JSX.Element[] {
        return this.props.MenuItems.map(value => <MenuItem { ... value } />);
    }

    public override render() {
        if (this.props && this.props.MenuItems) {
            return (
                <div className="pc-sidebar pc-links">
                    <nav>
                        <ul className="list-unstyled">
                            {this.renderMenu()}
                        </ul>
                    </nav>
                </div>
            );
        } else {
            return null;
        }
    }
}