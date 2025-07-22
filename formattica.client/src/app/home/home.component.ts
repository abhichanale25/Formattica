import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
features = [
    {
      title: 'Compressor',
      desc: 'Compress HTML, CSS, JS, or JSON efficiently.',
      icon: 'bi bi-arrow-down-circle',
    },
    {
      title: 'Comparison',
      desc: 'Compare files or code differences easily.',
      icon: 'bi bi-columns-gap',
    },
    {
      title: 'Formatter',
      desc: 'Beautify and structure your code cleanly.',
      icon: 'bi bi-braces',
    },
    {
      title: 'Conversion',
      desc: 'Convert code formats, units, and more.',
      icon: 'bi bi-shuffle',
    },
  ];
}
