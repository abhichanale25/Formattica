import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-formatter-tool',
  templateUrl: './formatter-tool.component.html',
  styleUrls: ['./formatter-tool.component.css'],
})
export class FormatterToolComponent {
  formatType: string = 'JSON';
  rawInput: string = '';
  formattedOutput: string = '';
  errorMessage: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    
  }

  formatContent() {
    this.formattedOutput = '';
    this.errorMessage = '';

    const payload = {
      content: this.rawInput,
      formatType: this.formatType,
    };

    this.http
      .post<any>('https://localhost:7108/api/Formatter/format', payload)
      .subscribe({
        next: (response) => {
          this.formattedOutput = response.formatted;
        },
        error: (err) => {
          this.errorMessage =
            err?.error?.message || 'An error occurred while formatting.';
        },
      });
  }

copied: boolean = false;

copyFormattedOutput() {
  if (this.formattedOutput) {
    navigator.clipboard.writeText(this.formattedOutput).then(() => {
      this.copied = true;
      setTimeout(() => this.copied = false, 2000); // hide after 2s
    });
  }
}
}
