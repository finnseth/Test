import {
    ActivatedRoute,
    NavigationEnd,
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
    selector: 'dua-slidingmenu',
    templateUrl: './slidingmenu.component.html',
    styleUrls: ['./slidingmenu.component.scss']
})
export class SlidingMenuComponent implements OnInit {

    items: MainMenuItem[];
    isOpen = false;
    topItem: MainMenuItem;

    constructor(
        public menuService: MainMenuService,
        private router: Router,
        private activatedRoute: ActivatedRoute
    ) {}

    ngOnInit() {
        this.items = this.menuService.items;
    }

    activateMenu () {
        if (this.menuService.topItem !== undefined) {
            this.topItem = this.menuService.topItem;
            this.topItem.expanded = true;
            for (const item of this.topItem.submenu) {
                item['expanded'] = true;
            }
            const activeItem = this.menuService.FindItemByRoute(this.topItem, this.router.url);
            if (activeItem !== null) {
                this.menuService.SetItemExpanded(activeItem);
            }
        }
    }

    openMenu(event: Event) {
        this.isOpen = true;
        this.activateMenu();
    }

    closeMenu(event: MainMenuItem) {
        this.isOpen = false;
    }

    handleClick(event, item: MainMenuItem) {
        if (!item.expanded) {
            this.menuService.SetItemExpanded(item);
        } else {
            this.menuService.CollapseItemWithChildren(item);
        }
    }

}
