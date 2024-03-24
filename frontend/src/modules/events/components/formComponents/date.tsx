import { Grid, InputLabel } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import { DateTimePicker } from '@mui/x-date-pickers';
import React, { useEffect } from 'react';
import dayjs, { Dayjs } from "dayjs";
import useSetDate from "../../store/hooks/eventDetails/useSetDate";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";

const DateField = () => {
    const eventStore = useEvent();
    const setDate = useSetDate();
    const disabled = eventStore.selectedEvent.uuid && !eventStore.selectedEvent.recurrency ? false : true;
    const [value, setValue] = React.useState<Dayjs | null>(dayjs());
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        setValue(dayjs(eventStore.selectedEvent.date));
    }, [eventStore.selectedEvent.date])

    const handleDateChange = (e : any) => {
        const value = e.toDate();

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/date', dayjs(e.toDate()))
            .then(() => {
                if(eventStore.selectedEvent)
                    setDate(eventStore.selectedEvent.uuid, value);
            })
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="date-input">Date</InputLabel>
            <DateTimePicker
                sx={{ width: '100%' }}
                value={value}
                onChange={handleDateChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default DateField;