import {
  Component,
  OnInit,
  ViewChild,
  ElementRef
} from '@angular/core';

import {
  MapOptions,
  SchemeData
} from '../../models';

import { MatSliderChange } from '@angular/material';
import { MapService } from '../../services';
import * as d3 from 'd3';

@Component({
  selector: 'home',
  templateUrl: 'home.component.html'
})
export class HomeComponent implements OnInit {
  private features: d3.Selection<Element, {}, any, any>;
  private state: d3.Selection<Element, {}, any, any>;
  private scale: d3.ScaleThreshold<number, string>;
  private zoomed = () => this.features && this.features.attr('transform', d3.event.transform);

  svg: d3.Selection<Element, {}, any, any>;
  creating = false;
  scheme = {} as SchemeData;

  options = {
    width: 780,
    height: 780
  } as MapOptions;

  zoom = d3.zoom()
    .scaleExtent([1, 10])
    .on('zoom', this.zoomed);

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

     this.state && this.colorMap();
  }

  resetMap = () => this.svg.transition()
    .duration(750)
    .call(this.zoom.transform, d3.zoomIdentity);

  createMap = async () => {
    this.creating = true;
    const res = await this.mapper.createMap(this.options);

    if (res && !res.hasError) {
      this.clearMap();
      this.drawMap(JSON.parse(res.result));
    }

    this.creating = false;
  }

  clearMap = () => d3.select(this.map.nativeElement)
    .select('svg')
    .remove();

  drawMap = (data: any) => {
    this.svg = d3.select(this.map.nativeElement)
      .append('svg')
      .style('width', `${this.options.width}px`)
      .style('height', `${this.options.height}px`);

    this.features = this.svg.append('g');

    this.state = this.features.selectAll('path')
      .data(data.features)
      .enter()
      .append('path')
      .attr('d', d3.geoPath());

    this.colorMap();
    this.svg.call(this.zoom);
  }

  colorMap = () => this.state
    .style('fill', (d: any) => this.scale(d.properties.density));
}
