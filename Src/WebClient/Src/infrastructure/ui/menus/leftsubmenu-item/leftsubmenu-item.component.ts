import { NavigationEnd, Router } from '@angular/router';
import {
    Component,
    ElementRef,
    HostBinding,
    HostListener,
    Input,
    OnInit,
    EventEmitter,
    Output,
    Renderer,
    animate,
    state,
    style,
    transition,
    trigger
} from '@angular/core';

import { MainMenuService, MainMenuItem } from './../../../services/mainmenu.service';

@Component({
    selector: 'dua-leftsubmenu-item',
    templateUrl: './leftsubmenu-item.component.html',
    styleUrls: ['./leftsubmenu-item.component.scss'],
    animations: [
        trigger('expanded', [
          state('true' , style({ color: '#ff0000' })),
          state('false', style({ maxHeight: 0, padding: 0, display: 'none' })),
          // transition
          transition('* => *', animate('300ms ease-out')),
        ])
    ],
})
export class LeftSubMenuItemComponent implements OnInit {

    @Input() item = <MainMenuItem>null; // see angular-cli issue #2034

    @Input() expanded: boolean;

    @Input() mobileStyle = false;

    @Output() onSelectedItem: EventEmitter<MainMenuItem> = new EventEmitter();

    constructor(
        private router: Router,
        private menuService: MainMenuService,
        private el: ElementRef,
        private renderer: Renderer
    ) {}


    ngOnInit(): void {
    }

    handleClick(event, item: MainMenuItem) {
        if (item.submenu === undefined || item.submenu === null) {
            if (item.route === undefined || item.route === null) {
                event.preventDefault();
                return;
            } else {
                this.menuService.SetItemActive(item);
                this.menuService.SetItemExpanded(item);
                this.onSelectedItem.emit(item);
                this.router.navigate([item.route]);
            }
        } else {
            this.menuService.SetItemActive(item);
            if (item.expanded) {
                this.menuService.CollapseItemWithChildren(item);
            } else {
                this.menuService.SetItemExpanded(item);
            }
            this.router.navigate([item.route]);
            event.preventDefault();
            return;
        }
    }

    emitSelectedItem(item: MainMenuItem) {
        this.onSelectedItem.emit(item);
    }
}
