import {
  Component,
  OnInit,
  ViewChild,
  ElementRef
} from '@angular/core';

import {
  MatSelectChange,
  MatSliderChange
} from '@angular/material';

import {
  MapOptions,
  SchemeData
} from '../../models';

import { MapService } from '../../services';
import * as d3 from 'd3';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  private scale: d3.ScaleThreshold<number, string>;

  creating = false;
  scheme = { } as SchemeData;

  options = {
    width: 960,
    height: 960
  } as MapOptions;

  @ViewChild('map', { static: false }) map: ElementRef<HTMLDivElement>;

  constructor(
    public mapper: MapService
  ) { }

  ngOnInit() {
    this.mapper.getStateData();
  }

  updateWidth = (event: MatSliderChange) => this.options.width = event.value;
  updateHeight = (event: MatSliderChange) => this.options.height = event.value;

  setScale = () => {
    this.scale = d3.scaleThreshold<number, string>()
      .domain([1, 10, 50, 200, 500, 1000, 2000, 4000])
      .range(this.scheme.scheme[9]);
  }

  createMap = async () => {
    this.creating = true;
    const res = await this.mapper.createMap(this.options);
    if (res && !res.hasError) {
      this.clearMap();
      this.drawMap(JSON.parse(res.result));
    }
    res && !res.hasError && console.log(JSON.parse(res.result));
    this.creating = false;
  }

  clearMap = () => d3.select(this.map.nativeElement)
    .select('svg')
    .remove();

  drawMap = (data: any) => d3.select(this.map.nativeElement)
    .append('svg')
    .style('width', `${this.options.width}px`)
    .style('height', `${this.options.height}px`)
    .selectAll('path')
    .data(data.features)
    .enter()
    .append('path')
    .attr('d', d3.geoPath())
    .style('fill', (d: any) => this.scale(d.properties.density));
}
