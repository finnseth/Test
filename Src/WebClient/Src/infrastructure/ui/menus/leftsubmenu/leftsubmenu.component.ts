import {
    ActivatedRoute,
    NavigationEnd,
    PRIMARY_OUTLET,
    Params,
    Router
} from '@angular/router';
import {
    Component,
    ElementRef,
    HostBinding,
    HostListener,
    Input,
    OnInit,
    Renderer,
    animate,
    state,
    style,
    transition,
    trigger
} from '@angular/core';

import {
    MainMenuItem,
    MainMenuService
} from './../../../services/mainmenu.service';

@Component({
    selector: 'dua-leftsubmenu',
    templateUrl: './leftsubmenu.component.html',
    styleUrls: ['./leftsubmenu.component.scss']
})
export class LeftSubMenuComponent implements OnInit {

    item: MainMenuItem;

    constructor(
        public menuService: MainMenuService,
        private router: Router,
        private activatedRoute: ActivatedRoute
    ) {}

    ngOnInit() {
        this.activateMenu();

        this.router.events.subscribe(event => {
            this.activateMenu();
        });
    }

    activateMenu () {
        if (this.menuService.topItem !== undefined) {
            this.item = this.menuService.topItem;
            for (const item of this.item.submenu) {
                item.expanded = true;
            }
            const activeItem = this.menuService.FindItemByRoute(this.item, this.router.url);
            if (activeItem !== null) {
                if (this.menuService.topItem.expanded) {
                    this.menuService.SetItemExpanded(activeItem);
                }
                this.menuService.SetItemActive(activeItem);
            }
        }
    }

    handleClick(event, item: MainMenuItem) {
        if (!item.route) {
            event.preventDefault();
            return;
        } else {
            this.menuService.SetItemActive(item);
            this.menuService.SetItemExpanded(item);
            this.router.navigate([item.route]);
        }
    }
}
