import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'dua-bluecard',
  templateUrl: './blueCard.component.html',
  styleUrls: ['./blueCard.component.scss']
})
export class BlueCardComponent implements OnInit {

  @Input() isComparing = false;

  constructor() {}

  ngOnInit() {

  }
}
