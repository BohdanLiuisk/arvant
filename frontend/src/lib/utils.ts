import { type ClassValue, clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { TinyColor } from '@ctrl/tinycolor';
import colors from 'tailwindcss/colors';
import type { Theme } from '@/assets/themes';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function convertToHex(str: string): string {
  return new TinyColor(str.replace(/deg|grad|rad|turn/g, '')).toHexString();
}

export function convertHslToHex(str: string | undefined): string {
  return convertToHex(`hsl(${str})`);
}

export function addAlphaToHex(hexColor: string, opacity: number): string {
  const tinyColor = new TinyColor(hexColor);
  const colorWithOpacity = tinyColor.setAlpha(opacity);
  return colorWithOpacity.toHex8String();
}

export function getNextColorInPalette(theme: Theme['name'], color: string): string {
  const colorPalette = colors[theme];
  const entries = Object.entries(colorPalette);
  entries.sort((a, b) => Number(a[0]) - Number(b[0]));
  const index = entries.findIndex(([key, val]) => val === color);
  if (index !== -1 && index + 1 < entries.length) {
    return entries[index + 1][1];
  } else {
    return '#0000';
  }
}
