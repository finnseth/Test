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
import { NavigationEnd, Router } from '@angular/router';

import { SelectItem } from 'primeng/primeng';

@Component({
    selector: 'dua-topmenu-item',
    templateUrl: './topmenu-item.component.html',
    styleUrls: ['./topmenu-item.component.scss'],
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
export class TopMenuItemComponent implements OnInit {
  @Input() item = <MainMenuItem>null;  // see angular-cli issue #2034
  @HostBinding('class.parent-is-popup')
  @Input() menuPosition = 'left';
  isActiveRoute = false;

  mouseInItem = false;
  mouseInPopup = false;
  popupTop = 0;
  popupLeft = 0;

  constructor(private router: Router,
    private menuService: MainMenuService,
    private el: ElementRef,
    private renderer: Renderer) {
  }

  checkActiveRoute(route: string) {
    this.updateMenu(route);
  }

  ngOnInit(): void {
    this.checkActiveRoute(this.router.url);

    this.router.events
      .subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.checkActiveRoute(this.router.url);
        }
      });
  }

  private updateMenu(route: string): void {
    const item = this.findItemByRoute(this.item, route);
    if (item !== null) {
      this.isActiveRoute = true;
      this.menuService.SetSelectedItem(this.item);
    } else {
      this.isActiveRoute = false;
    }
  }

    private findItemByRoute(item: MainMenuItem, url: string): MainMenuItem {
        if (item.route === url) {
            return item;
        } else {
            if (item.submenu !== undefined) {
                if (item.submenu !== null) {
                    for (const sub of item.submenu) {
                        const output = this.findItemByRoute(sub, url);
                        if (output !== null) {
                            return output;
                        }
                    }
                }
            }
        }
        return null;
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
        }
    }
}
