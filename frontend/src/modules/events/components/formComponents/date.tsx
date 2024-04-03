import { Grid, InputLabel } from "@mui/material"
import { DateTimePicker } from '@mui/x-date-pickers';
import React, { useEffect } from 'react';
import dayjs, { Dayjs } from "dayjs";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { SelectedEventState, setSelectedEventDate } from "../../store/selectedEventSlice";
import { setEventDate } from "../../store/eventsSlice";

const DateField = () => {
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();
    const disabled = event.event.uuid && !event.event.recurrency ? false : true;
    const [value, setValue] = React.useState<Dayjs | null>(dayjs());
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        setValue(dayjs(event.event.date));
    }, [event.event.date])

    const handleDateChange = (e : any) => {
        const value = e.toDate();

        if(value) {
            updateEvent(event.event.uuid, '/date', dayjs(e.toDate()))
            .then(() => {
                if(event){
                    dispatch(setSelectedEventDate(value));
                    dispatch(setEventDate({eventUuid: event.event.uuid, date: value}));
                }
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