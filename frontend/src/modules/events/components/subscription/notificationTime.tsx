import React, { useRef } from 'react';
import { InputLabel, Grid } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

interface NotificationDateProps {
    setNotificationDate: any;
    defaultValue: Date | null;
}

const NotificationDate = (props: NotificationDateProps) => {
    const { setNotificationDate, defaultValue } = props;
    const inputRef = useRef<any>();

    const handleDateChange = (event: any) => {
        setNotificationDate(event.toDate());
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="notification-date-input">Notification Date</InputLabel>
            <DateTimePicker
                sx={{ width: '100%' }}
                defaultValue={dayjs(defaultValue)}
                inputRef={inputRef}
                onChange={handleDateChange}
            />
        </Grid>
    )
}

export default NotificationDate;