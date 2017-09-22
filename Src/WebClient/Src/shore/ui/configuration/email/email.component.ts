import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AuthenticationService } from './../../../services/authentication.service';

// import { MenuItem } from 'primeng/primeng';


@Component({
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.scss']
})
export class EmailComponent implements OnInit {

    // private menuItems: MenuItem[] = [];

    constructor(private authService: AuthenticationService, private route: ActivatedRoute) {
    }

    public ngOnInit(): void {
    }
}
