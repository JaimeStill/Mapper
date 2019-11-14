import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { SnackerService } from './snacker.service';

import {
  schemeBrBG,
  schemePRGn,
  schemePiYG,
  schemePuOr,
  schemeRdBu,
  schemeRdGy,
  schemeRdYlBu,
  schemeRdYlGn,
  schemeSpectral,
  schemeBlues,
  schemeGreens,
  schemeGreys,
  schemeOranges,
  schemePurples,
  schemeReds,
  schemeBuGn,
  schemeBuPu,
  schemeGnBu,
  schemeOrRd,
  schemePuBuGn,
  schemePuBu,
  schemePuRd,
  schemeRdPu,
  schemeYlGnBu,
  schemeYlGn,
  schemeYlOrBr,
  schemeYlOrRd
} from 'd3';

import {
  ConsoleOutput,
  MapOptions,
  SchemeData,
  StateData
} from '../models';

@Injectable()
export class MapService {
  private states = new BehaviorSubject<StateData[]>(null);

  private schemes = new BehaviorSubject<SchemeData[]>([
    { name: 'Spectral', scheme: schemeSpectral },
    { name: 'Blues', scheme: schemeBlues },
    { name: 'Greens', scheme: schemeGreens },
    { name: 'Greys', scheme: schemeGreys },
    { name: 'Oranges', scheme: schemeOranges },
    { name: 'Purples', scheme: schemePurples },
    { name: 'Reds', scheme: schemeReds },
    { name: 'Red | Blue', scheme: schemeRdBu },
    { name: 'Red | Grey', scheme: schemeRdGy },
    { name: 'Red | Purple', scheme: schemeRdPu },
    { name: 'Blue | Green', scheme: schemeBuGn },
    { name: 'Blue | Purple', scheme: schemeBuPu },
    { name: 'Green | Blue', scheme: schemeGnBu },
    { name: 'Orange | Red', scheme: schemeOrRd },
    { name: 'Purple | Green', scheme: schemePRGn },
    { name: 'Purple | Orange', scheme: schemePuOr },
    { name: 'Purple | Blue', scheme: schemePuBu },
    { name: 'Purple | Red', scheme: schemePuRd },
    { name: 'Yellow | Green', scheme: schemeYlGn },
    { name: 'Brown | Blue-Green', scheme: schemeBrBG },
    { name: 'Pink | Yellow-Green', scheme: schemePiYG },
    { name: 'Red | Yellow | Blue', scheme: schemeRdYlBu },
    { name: 'Red | Yellow | Green', scheme: schemeRdYlGn },
    { name: 'Purple | Blue | Green', scheme: schemePuBuGn },
    { name: 'Yellow | Green | Blue', scheme: schemeYlGnBu },
    { name: 'Yellow | Orange | Brown', scheme: schemeYlOrBr },
    { name: 'Yellow | Orange | Red', scheme: schemeYlOrRd }
  ]);

  states$ = this.states.asObservable();
  schemes$ = this.schemes.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService
  ) { }

  getStateData = () => this.http.get<StateData[]>(`/api/map/getStateData`)
    .subscribe(
      data => this.states.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  createMap = (options: MapOptions): Promise<ConsoleOutput> =>
    new Promise((resolve) => {
      this.http.post<ConsoleOutput>(`/api/map/createMap`, options)
        .subscribe(
          output => {
            !output.hasError && this.snacker.sendSuccessMessage(`${options.data.name}.json created`);
            resolve(output);
          },
          err => {
            this.snacker.sendErrorMessage(err.error);
            resolve(null);
          }
        );
    });
}
