import React from 'react';
import DOMPurify from "dompurify";
import MainMenu from './MainMenu';
import { PageApiModel } from './ApiModels'
import axios from 'axios';
import SideMenu from './SideMenu';

interface IPageLoader {
    loadPage(path: string): void;
}

export var pageLoader: IPageLoader;

class AppState {
    static EmptyModel: PageApiModel = {
        MainMenu: {
            MenuItems: []
        },
        SideMenu: {
            MenuItems: []
        },
        PageTitle: "",
        Title: "",
        Content: ""
    }

    isLoaded: boolean = false;
    error: string = "";
    model: PageApiModel = AppState.EmptyModel;
}

export default class App extends React.Component<any, AppState> implements IPageLoader {
    apiRoot = "http://localhost:5004";
    srcSingleQuoteExp = new RegExp("src='/");

    constructor(props: any) {
        super(props);
        this.state = new AppState();
        pageLoader = this;
        window.addEventListener('popstate', (ev: PopStateEvent) => {
            if (ev.state) {
                this.loadPage(ev.state, false);
            }
        });
    }

    public loadPage(path: string, pushNewState = true): void {
        axios.get<PageApiModel>(this.apiRoot + "/Api/Page?path=" + path)
        .then(response => {
            response.data.Content = DOMPurify.sanitize(response.data.Content, {
                USE_PROFILES: { html: true },
            });
            response.data.Content = response.data.Content
                .replaceAll("src='/", "src='" + this.apiRoot + "/")
                .replaceAll("src=\"/", "src=\"" + this.apiRoot + "/");
            document.title = response.data.PageTitle;
            this.setState({
                isLoaded: true,
                model: response.data
            });
            if (pushNewState) {
                window.history.pushState(path, "", path);
            }
        })
        .catch(reason => {
            this.setState({
                isLoaded: true,
                error: reason.error
            });
        })
    }

    public override componentDidMount() {
        this.loadPage(window.location.pathname);
    }

    private getContainerClassName() {
        let className = "container-xxl";
        if (this.state.model.SideMenu && this.state.model.SideMenu.MenuItems) {
            className += " pc-layout";
        }
        return className;
    }

    public override render() {
        if (this.state.error) {
            return <div>Error: {this.state.error}</div>;
        } else if (!this.state.isLoaded) {
            return <div>Loading...</div>;
        } else {
            return (
                <div>
                    <MainMenu {... this.state.model.MainMenu} />
                    <div className={this.getContainerClassName()}>
                        <SideMenu {... this.state.model.SideMenu} />
                        <main role="main" className="content">
                            <h1>{this.state.model.Title}</h1>
                            <div dangerouslySetInnerHTML={{ __html: this.state.model.Content }}></div>
                        </main>
                    </div>
                </div>
            );
        }
    }
}