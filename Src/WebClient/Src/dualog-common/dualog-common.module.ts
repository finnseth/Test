import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Services
import { ConfigurationReader } from './services/configuration-reader.service';
import { MainMenuService } from './services/mainmenu.service';
import { ScreenService } from './services/screen.service';
import { SchemaFormBuilder } from './services/schema-form-builder.service';
import { SearchService } from './search/search.service';
import { ShipSearchProvider } from './search/shipsearchprovider';

// Components
import { BodyComponent } from './components/body/body.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { CommonModule } from '@angular/common';
import { ContentComponent } from './components/content/content.component';
import { DuaCheckboxModule } from './components/dua-checkbox/dua-checkbox.module';
import { GlogoComponent } from './components/glogo/glogo.component';
import { HeaderComponent } from './components/header/header.component';
import { LogoComponent } from './components/logo/logo.component';
import { MenuComponent } from './menus/menu/menu.component';
import { MenuItemComponent } from './menus/menu-item/menu-item.component';
import { NgModule } from '@angular/core';
import { OverlayPanelModule } from 'primeng/primeng'
import { PopupMenuComponent } from './menus/popup-menu/popup-menu.component';
import { PopupTopMenuComponent } from './menus/popup-topmenu/popup-topmenu.component';
import { RouterModule } from '@angular/router';
import { ScreenBelowLarge } from './directives/screen-below-large.directive';
import { ScreenLarge } from './directives/screen-large.directive';
import { SearchboxComponent } from './components/searchbox/searchbox.component';
import { TopMenuComponent } from './menus/topmenu/topmenu.component';
import { TopMenuItemComponent } from './menus/topmenu-item/topmenu-item.component';
import { UserboxComponent } from './components/userbox/userbox.component';
import { VerticalBarComponent } from './components/vertical-bar/vertical-bar.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    OverlayPanelModule,
    DuaCheckboxModule
  ],

  providers: [
    SchemaFormBuilder,
    ConfigurationReader,
    ScreenService,
    SearchService,
    MainMenuService,
    ShipSearchProvider
  ],

  declarations: [
    BodyComponent,
    BreadcrumbComponent,
    HeaderComponent,
    ContentComponent,
    LogoComponent,
    GlogoComponent,
    VerticalBarComponent,
    MenuComponent,
    MenuItemComponent,
    TopMenuComponent,
    TopMenuItemComponent,
    PopupMenuComponent,
    PopupTopMenuComponent,
    SearchboxComponent,
    UserboxComponent,
    ScreenLarge,
    ScreenBelowLarge
  ],

  exports: [
    BodyComponent,
    ScreenLarge,
    ScreenBelowLarge
  ]
})
export class DualogCommonModule { }
