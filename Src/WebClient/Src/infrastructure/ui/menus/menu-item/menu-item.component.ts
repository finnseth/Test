import { NavigationEnd, Router } from '@angular/router';
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
} from 'infrastructure/services/mainmenu.service';


@Component({
    selector: 'dua-menu-item',
    templateUrl: './menu-item.component.html',
    styleUrls: ['./menu-item.component.scss'],
    animations: [
        trigger('visibilityChanged', [
            transition(':enter', [
                // :enter is alias to 'void => *'
                style({ opacity: 0 }),
                animate(250, style({ opacity: 1 }))
            ]),
            transition(':leave', [
                // :leave is alias to '* => void'
                animate(100, style({ opacity: 0 }))
            ])
        ])
    ]
})
export class MenuItemComponent implements OnInit {
    @Input() item = <MainMenuItem>null; // see angular-cli issue #2034
    @HostBinding('class.parent-is-popup')
    @Input()
    parentIsPopup = true;
    @Input() menuPosition = 'left';
    isActiveRoute = false;

    mouseInItem = false;
    mouseInPopup = false;
    popupTop = 0;
    popupLeft = 0;

    constructor(
        private router: Router,
        private menuService: MainMenuService,
        private el: ElementRef,
        private renderer: Renderer
    ) {}

    checkActiveRoute(route: string) {
        this.isActiveRoute = route === this.item.route;
    }

    ngOnInit(): void {
        this.checkActiveRoute(this.router.url);

        this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.checkActiveRoute(event.url);
                // console.log(event.url + ' ' + this.item.route + ' ' + this.isActiveRoute);
            }
        });
    }

    @HostListener('click', ['$event'])
    onClick(event): void {
        event.stopPropagation();

        if (this.item.route) {
            // force horizontal menus to close by sending a mouseleave event
            const newEvent = new MouseEvent('mouseleave', { bubbles: true });
            this.renderer.invokeElementMethod(
                this.el.nativeElement,
                'dispatchEvent',
                [newEvent]
            );
            this.router.navigate([this.item.route]);
        }
    }

    onPopupMouseEnter(event): void {
        this.mouseInPopup = true;
    }

    onPopupMouseLeave(event): void {
        this.mouseInPopup = false;
    }

    @HostListener('mouseleave', ['$event'])
    onMouseLeave(event): void {
        this.mouseInItem = false;
    }

    @HostListener('mouseenter')
    onMouseEnter(): void {
        this.popupTop = this.el.nativeElement.offsetHeight;
        if (this.menuPosition === 'right') {
            this.popupLeft = -162 + this.el.nativeElement.offsetWidth;
        }

        if (this.item.submenu) {
            this.mouseInItem = true;
            if (this.parentIsPopup) {
                this.popupLeft = 160;
                this.popupTop = 0;
            }
        }
    }
}
