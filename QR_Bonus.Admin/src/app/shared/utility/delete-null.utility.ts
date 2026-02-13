export class DeleteNullsUtility {
  static deteteNulls(obj: any) {
    Object.keys(obj).forEach((k) => obj[k] == null && delete obj[k]);
  }
}
