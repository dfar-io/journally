export class Entry {
  id: string;
  datetime: Date;
  content: string;
  title?: string;
}

export class EntryResolved {
  entry: Entry;
  error?: any;
}
