import React from 'react';
import { MenuApiModel } from './ApiModels'
import MenuItem from './MenuItem';

export default class MainMenu extends React.Component<MenuApiModel, any> {
    public constructor(props: MenuApiModel) {
        super(props)
    }

    public renderMenu(): JSX.Element[] {
        return this.props.MenuItems.map(value => <MenuItem { ... value } />);
    }

    public override render() {
        return (
            <header>
                <nav className="navbar pc-links navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                    <div className="container">
                        <a className="navbar-brand" href="/"><i className="bi-house"></i></a>
                        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                            <ul className="pc-main-menu navbar-nav flex-grow-1">
                                {this.renderMenu()}
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
        );
    }
}