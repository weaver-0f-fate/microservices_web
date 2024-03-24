

export const compareTimes = (time1: Date, time2: Date) => {
    if (time1.getHours() > time2.getHours()) {
        return 1;
    } else if (time1.getHours() < time2.getHours()) {
        return -1;
    } else {
        if (time1.getMinutes() > time2.getMinutes()) {
            return 1;
        } else if (time1.getMinutes() < time2.getMinutes()) {
            return -1;
        } else {
            return 0;
        }
    }
}

export const getLocalDate = (dateString: any) => {;
    const dateComponents = dateString.split('T');
    const datePart = dateComponents[0];
    const timePart = dateComponents[1];

    // Split the date component to get year, month, and day
    const dateParts = datePart.split('-');
    const year = parseInt(dateParts[0]);
    const month = parseInt(dateParts[1]) - 1; // Months are 0-indexed, so subtract 1
    const day = parseInt(dateParts[2]);

    // Split the time component to get hours, minutes, and seconds
    const timeParts = timePart.split(':');
    const hours = parseInt(timeParts[0]);
    const minutes = parseInt(timeParts[1]);
    const seconds = parseInt(timeParts[2]);

    // Create a UTC date using Date.UTC() with extracted components
    const utcDate = new Date(Date.UTC(year, month, day, hours, minutes, seconds));
    return utcDate;
}

export const formatTime = (date: Date) : string => {
    const hours = date.getUTCHours();
    const minutes = date.getUTCMinutes();
    const seconds = date.getUTCSeconds();
  
    return `${hours}:${minutes}:${seconds}`;
  }