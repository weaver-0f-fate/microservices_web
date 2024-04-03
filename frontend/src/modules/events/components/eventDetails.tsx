import { Box, Container, Grid } from '@mui/material';
import React, { useEffect } from 'react';
import TitleField from './formComponents/title';
import ImageUrlField from './formComponents/imageUrl';
import PlaceField from './formComponents/place';
import AdditionalInfoField from './formComponents/additionalInfo';
import DescriptionField from './formComponents/description';
import DateField from './formComponents/date';
import CategoryField from './formComponents/category';
import { useGetEvent } from '../fetch/useGetEvent';
import { useDispatch, useSelector } from 'react-redux';
import { Event } from '../../../shared/models/events';
import { SelectedEventState, setSelectedEvent } from '../store/selectedEventSlice';
import { getLocalDate } from '../../../shared/utilities/dateFunctions';

const EventDetails = () => {
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();
    const getEvent = useGetEvent();

    useEffect(() => {
        if(event) {
            getEvent(event.event.uuid)
                .then(result => {
                    dispatch(setSelectedEvent({
                        uuid: result.uuid,
                        category: result.category,
                        title: result.title,
                        imageUrl: result.imageUrl,
                        description: result.description,
                        place: result.place,
                        date: result.date ? getLocalDate(result.date) : new Date(),
                        additionalInfo: result.additionalInfo,
                        recurrency: result.recurrency,
                    } as Event))
                })
                .catch(error => console.error(error))
        }
    }, [event.event.uuid])

    return (
        <Box>
            <Container maxWidth="sm" sx={{ p: 5 }}>
                <Grid container spacing={2}>
                    <CategoryField />
                    <TitleField />
                    <ImageUrlField />
                    <PlaceField />
                    <DescriptionField />
                    <AdditionalInfoField />
                    <DateField />
                </Grid>
            </Container>
        </Box>
    )
}



export default EventDetails;