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
import { MainMenuItem, MainMenuService } from '../../services/mainmenu.service';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'dua-topmenu-item',
  templateUrl: './topmenu-item.component.html',
  styleUrls: ['./topmenu-item.component.scss'],
  animations: [
    trigger('visibilityChanged', [
      transition(':enter', [   // :enter is alias to 'void => *'
        style({ opacity: 0 }),
        animate(250, style({ opacity: 1 }))
      ]),
      transition(':leave', [   // :leave is alias to '* => void'
        animate(100, style({ opacity: 0 }))
      ])
    ])
  ]
})
export class TopMenuItemComponent implements OnInit {
  @Input() item = <MainMenuItem>null;  // see angular-cli issue #2034
  @HostBinding('class.parent-is-popup')
  @Input() parentIsPopup = true;
  @Input() menuPosition = 'left';
  @Input() isTopNode = false;
  isActiveRoute = false;

  mouseInItem = false;
  mouseInPopup = false;
  popupTop = 0;
  popupLeft = 0;
  title: string;

  constructor(private router: Router,
    private menuService: MainMenuService,
    private el: ElementRef,
    private renderer: Renderer) {
  }

  checkActiveRoute(route: string) {
    if (this.isTopNode) {
      this.updateMenu(this.item.submenu, route);
    }
  }

  ngOnInit(): void {
    console.log('init: ' + this.router.url);
    this.checkActiveRoute(this.router.url);

    this.router.events
      .subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.checkActiveRoute(event.url);
        }
      });
  }

  private updateMenu(items: MainMenuItem[], route: string): void {
    let categoryFound = false;
    for (const item of items) {
      if ( route.search(item.text.toLowerCase()) > -1) {
        this.title = item.text;
        this.item.route = item.route;
        this.menuService.SetSelectedItem(this.title);
        categoryFound = true;
      }
    }
    // Fallback
    if (!categoryFound) {
      this.title = 'Home';
      this.item.route = '/';
      this.menuService.SetSelectedItem(this.title);
    }
  }

  @HostListener('click', ['$event'])
  onClick(event): void {

    event.stopPropagation();

    if (this.item.route) {

      // force horizontal menus to close by sending a mouseleave event
      const newEvent = new MouseEvent('mouseleave', { bubbles: true });
      this.renderer.invokeElementMethod( this.el.nativeElement, 'dispatchEvent', [newEvent] );
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
    console.log(this.menuPosition);
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
