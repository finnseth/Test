import { APP_INITIALIZER, NgModule } from '@angular/core';
import {
  AutoCompleteModule,
  ButtonModule,
  MenuItem,
  OverlayPanelModule,
  PanelMenuModule
} from 'primeng/primeng';
import { ConfigurationReader, DualogCommonModule } from 'dualog-common';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ConnectionSuiteShoreModule } from 'connection-suite-shore/connection-suite-shore.module';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { LogoutModule } from 'connection-suite/components/logout/logout.module';
import { RouterModule } from '@angular/router';
import { RoutesearchComponent } from 'connection-suite-shore/routesearch/routesearch.component';
import { UnauthorizedComponent } from 'app/unauthorized/unauthorized.component';
import { routing } from './app.routing';

export function loadConfig(config: ConfigurationReader) {
    return  () => config.load();
}

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    DualogCommonModule,
    LogoutModule,
    BrowserAnimationsModule,
    PanelMenuModule,
    ButtonModule,
    AutoCompleteModule,
    OverlayPanelModule,
    ConnectionSuiteShoreModule
  ],

  declarations: [
    AppComponent,
    LoginComponent,
    LogoutComponent,
    RoutesearchComponent,
    UnauthorizedComponent
  ],

  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [ConfigurationReader],
      multi: true
    }
  ],

  bootstrap: [ AppComponent ]
})

export class AppModule { }
