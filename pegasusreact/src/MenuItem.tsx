import React from 'react';
import { MenuItemApiModel } from './ApiModels'
import { pageLoader } from './App';

export default class MenuItem extends React.Component<MenuItemApiModel, any> {
    private static itemKey = 0;

    private getItemClassName(item: MenuItemApiModel): string {
        let className: string = "btn btn-link nav-link text-dark";
        if (item.Active) {
            className += " active";
        }
        return className;
    }

    private renderSubmenu(items: MenuItemApiModel[], level: number): JSX.Element | null {
        if (items) {
            return (
                <ul className={"level-" + level.toString()}>
                    {items.map((value) => this.renderMenuItem(value, level))}
                </ul>
            )
        }
        else {
            return null;
        }
    }

    private renderMenuItem(item: MenuItemApiModel, level: number): JSX.Element {
        return (
            <li key={(MenuItem.itemKey++).toString()} className="nav-item">
                <button className={this.getItemClassName(item)} onClick={() => { pageLoader.loadPage(item.Url) }} >{item.Title}</button>
                <a className="hidden" href={item.Url}>{item.Title}</a>
                {this.renderSubmenu(item.SubMenuItems, level++)}
            </li>
        );
    }

    public override render() {
        return this.renderMenuItem(this.props, 1);
    }
}