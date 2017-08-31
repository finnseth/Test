import { ActivatedRoute, Router, Routes } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { MenuService } from 'connection-suite-shore/services/menu.service';
import { configurationRoutes } from 'modules/configuration/configuration.routing';
import { dualogRoutes } from 'modules/dualog/dualog.routing';
import { informationRoutes } from 'modules/information/information.routing';
import { routes } from 'app/app.routing';

@Component({
    selector: 'dua-routesearch',
    templateUrl: './routesearch.component.html',
    styleUrls: ['./routesearch.component.css']
})
export class RoutesearchComponent implements OnInit {

    private searchSet: IRouteSearch[];
    private selectedRoute: IRouteSearch;
    private filteredRoutes: IRouteSearch[];

    constructor(private menuService: MenuService, private route: ActivatedRoute, private router: Router) {
    }

    public ngOnInit(): void {
        this.searchSet = [];
        this.buildRouteInformation(informationRoutes);
        this.buildRouteInformation(configurationRoutes);
        this.buildRouteInformation(dualogRoutes);
    }

    private buildRouteInformation(routes: Routes, parentPath: string = null): void {
        for (const route of routes) {
            if (route.data !== undefined) {
                if (route.data.permissions !== undefined) {
                    if (route.data.label !== undefined) {
                        if (this.menuService.GetMenuAccess(route.data.permissions)) {
                            // If any parent path add it to the path
                            let path = route.path;
                            if (parentPath !== null) {
                                path = parentPath + '/' + path;
                            }

                            // console.log(path);
                            this.searchSet.push({
                                label: route.data.label,
                                icon: route.data.icon,
                                routerLink: path,
                                description: route.data.description,
                                tag: route.data.label,
                                rank: 100
                            });

                            if (route.data.meta !== undefined) {
                                for (const meta of route.data.meta) {
                                    this.searchSet.push({
                                        label: route.data.label,
                                        icon: route.data.icon,
                                        routerLink: path,
                                        description: route.data.description,
                                        tag: meta.tag,
                                        rank: meta.rank
                                    });
                                }
                            }

                            if (route.children !== undefined) {
                                this.buildRouteInformation(route.children, path);
                            }
                        };
                    }
                }
            }
            if (route.children !== undefined) {
                if (route.data !== undefined) {
                    if (route.data.path !== undefined) {
                        this.buildRouteInformation(route.children, route.data.path);
                    }
                }
            }
        }
    }

    public filterRoutes(event: any) {
        this.filteredRoutes = [];
        for (let i = 0; i < this.searchSet.length; i++) {
            const route = this.searchSet[i];
            if (route.tag.toLowerCase().indexOf(event.query.toLowerCase()) > -1) {
                let routeExists = false;
                for (const r of this.filteredRoutes) {
                    if (r.routerLink === route.routerLink) {
                        routeExists = true;
                        if (r.rank < route.rank) {
                            r.rank = route.rank
                        }
                    }
                }
                if (!routeExists) {
                    this.filteredRoutes.push(route);
                }
                this.filteredRoutes = this.filteredRoutes.sort( (a: IRouteSearch, b: IRouteSearch) => { return b.rank - a.rank } );
            }
        }
    }

    public selectRoute(route: IRouteSearch) {
        this.router.navigate([route.routerLink]);
        this.selectedRoute = null;
    }
}

export interface IRouteSearch {
    label: string;
    icon: string;
    routerLink: string;
    description: string;
    tag: string;
    rank: number;
}
