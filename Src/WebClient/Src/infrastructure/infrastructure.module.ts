import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ConfigurationReader } from './services/configuration-reader.service';
import { DuaCheckboxModule } from './ui/dua-checkbox/dua-checkbox.module';
import { ScreenBelowLarge } from './directives/screen-below-large.directive';
import { ScreenLarge } from './directives/screen-large.directive';
import { TopMenuComponent } from './ui/menus/topmenu/topmenu.component';
import { TopMenuItemComponent } from './ui/menus/topmenu-item/topmenu-item.component';
import { MenuComponent } from './ui/menus/menu/menu.component';
import { PopupMenuComponent } from './ui/menus/popup-menu/popup-menu.component';
import { MenuItemComponent } from './ui/menus/menu-item/menu-item.component';
import { MainMenuService } from './services/mainmenu.service';
import { ScreenService } from './services/screen.service';
import { SchemaFormBuilder } from './services/schema';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    DuaCheckboxModule,
  ],

  providers: [
    ConfigurationReader,
    MainMenuService,
    ScreenService,
    SchemaFormBuilder
  ],

  declarations: [
    ScreenBelowLarge,
    ScreenLarge,
    TopMenuComponent,
    TopMenuItemComponent,
    MenuComponent,
    PopupMenuComponent,
    MenuItemComponent
  ],

  exports: [
    ScreenBelowLarge,
    ScreenLarge,
    TopMenuComponent,
    TopMenuItemComponent,
    MenuComponent,
    MenuItemComponent
  ]
})
export class InfrastructureModule { }
