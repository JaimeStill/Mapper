import { CoreService } from './core.service';
import { MapService } from './map.service';
import { SnackerService } from './snacker.service';
import { ThemeService } from './theme.service';

export const Services = [
  CoreService,
  MapService,
  SnackerService,
  ThemeService
];

export * from './core.service';
export * from './map.service';
export * from './snacker.service';
export * from './theme.service';
